using AgileFood.Application.Dtos.Consumptions;
using AgileFood.Application.Interfaces.Consumptions;
using AgileFood.Application.Mappings.Consumptions;
using AgileFood.Business.Exceptions;
using AgileFood.Business.Interfaces;
using AgileFood.Business.Models.Consumptions;
using AgileFood.Business.Models.Stock;
using AgileFood.Business.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AgileFood.Application.Services.Consumptions;

public class ConsumptionService : IConsumptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly TimeProvider _timeProvider;

    public ConsumptionService(
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher,
        TimeProvider timeProvider)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _timeProvider = timeProvider;
    }

    public async Task<ConsumptionResultDto> RegisterConsumptionAsync(RegisterConsumptionDto dto)
    {
        var user = await ValidateUserAndPinAsync(dto.EmployeeCode, dto.Pin);
        return await RegisterCartForUserAsync(user, dto.Items);
    }

    private async Task<User> ValidateUserAndPinAsync(string employeeCode, string pin)
    {
        var user = await _unitOfWork.UserRepository.GetByEmployeeCodeAsync(employeeCode);

        if (user is null)
            throw new DomainException("Usuário não encontrado.");

        if (!user.IsActive)
            throw new DomainException("Usuário não está ativo.");

        var now = _timeProvider.GetUtcNow().UtcDateTime;

        if (user.IsPinLocked(now))
            throw new AccountLockedException(
                "PIN bloqueado temporariamente por excesso de tentativas inválidas. Tente novamente mais tarde.");

        if (string.IsNullOrWhiteSpace(pin) || pin.Length != 4 || !pin.All(char.IsDigit))
            throw new DomainException("PIN invalido.");

        var verification = _passwordHasher.VerifyHashedPassword(user, user.TransactionPinHash, pin);

        if (verification == PasswordVerificationResult.Failed)
        {
            user.RegisterFailedPinAttempt(now);
            await _unitOfWork.CommitAsync();
            throw new DomainException("PIN invalido.");
        }

        user.ResetPinAttempts();
        return user;
    }

    private async Task<ConsumptionResultDto> RegisterCartForUserAsync(User user, IReadOnlyCollection<RegisterConsumptionItemDto> items)
    {
        if (items is null || items.Count == 0)
            throw new DomainException("O carrinho está vazio.");

        var groupedItems = items
            .GroupBy(i => i.ProductId)
            .Select(g => new RegisterConsumptionItemDto(g.Key, g.Sum(i => i.Quantity)))
            .ToList();

        if (groupedItems.Any(i => i.ProductId <= 0 || i.Quantity <= 0))
            throw new DomainException("O carrinho possui itens invalidos.");

        var nowUtc = _timeProvider.GetUtcNow().UtcDateTime;
        var consumption = new Consumption(user.Id, nowUtc);


        foreach (var item in groupedItems)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
            if (product is null)
                throw new DomainException("Produto nao encontrado.");

            if (!product.IsActive)
                throw new DomainException($"Produto '{product.Name}' nao esta ativo.");

            var stockItems = (await _unitOfWork.StockItemRepository.GetAvailableByProductIdAsync(item.ProductId)).ToList();
            if (stockItems.Sum(s => s.Quantity) < item.Quantity)
                throw new DomainException($"Estoque insuficiente para '{product.Name}'.");

            consumption.AddItem(product.Id, product.Name, product.Price, item.Quantity);
        }

        _unitOfWork.ConsumptionRepository.Add(consumption);

        foreach (var item in consumption.Items)
        {
            var remainingQuantity = item.Quantity;
            var stockItems = (await _unitOfWork.StockItemRepository.GetAvailableByProductIdAsync(item.ProductId)).ToList();

            foreach (var stockItem in stockItems)
            {
                if (remainingQuantity == 0)
                    break;

                var quantityToRemove = Math.Min(stockItem.Quantity, remainingQuantity);

                stockItem.RegisterExit(
                    quantityToRemove,
                    nowUtc,
                    StockMovementOrigin.Consumption,
                    $"Consumo #{user.Name} - {item.ProductName}",
                    consumption
                );

                remainingQuantity -= quantityToRemove;
            }

            if (remainingQuantity > 0)
                throw new DomainException($"Estoque insuficiente para '{item.ProductName}'.");
        }

        await _unitOfWork.CommitAsync();
        return consumption.MapToConsumptionDto();
    }

    public async Task<IEnumerable<ConsumptionResultDto>> GetByUserAsync(long userId)
    {
        var consumptions = await _unitOfWork.ConsumptionRepository.GetByUserIdAsync(userId);
        return consumptions.Select(c => c.MapToConsumptionDto());
    }

    public async Task<IEnumerable<MonthlyConsumptionSummaryDto>> GetMonthlySummaryByUserAsync(long userId)
    {
        var consumptions = await _unitOfWork.ConsumptionRepository.GetByUserIdAsync(userId);

        return consumptions
            .GroupBy(c => new { c.ReferenceYear, c.ReferenceMonth })
            .Select(g => new MonthlyConsumptionSummaryDto(g.Key.ReferenceYear, g.Key.ReferenceMonth, g.Sum(c => c.TotalPrice), g.Count()))
            .OrderByDescending(s => s.ReferenceYear)
            .ThenByDescending(s => s.ReferenceMonth)
            .ToList();
    }
}
