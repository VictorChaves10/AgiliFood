using System.Security.Cryptography;
using AgileFood.Application.Dtos.Users;
using AgileFood.Application.Interfaces.Users;
using AgileFood.Application.Mappings.Users;
using AgileFood.Business.Exceptions;
using AgileFood.Business.Interfaces;
using AgileFood.Business.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace AgileFood.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly TimeProvider _timeProvider;

    public UserService(
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher,
        TimeProvider timeProvider)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _timeProvider = timeProvider;
    }

    public async Task<UserResultDto> CreateAsync(CreateUserDto dto)
    {
        var existing = await _unitOfWork.UserRepository.GetByEmailAsync(dto.Email);
        if (existing is not null)
            throw new DomainException("E-mail inválido.");

        var normalizedCpf = User.NormalizeCpf(dto.Cpf);
        var existingCpf = await _unitOfWork.UserRepository.GetByCpfAsync(normalizedCpf);
        if (existingCpf is not null)
            throw new DomainException("CPF inválido.");

        var passwordHash = _passwordHasher.HashPassword(null!, dto.Password);
        var transactionPinHash = _passwordHasher.HashPassword(null!, dto.TransactionPin);
        var user = new User(dto.Name, dto.Email, normalizedCpf, passwordHash, transactionPinHash, dto.Role,
            _timeProvider.GetUtcNow().UtcDateTime);

        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.CommitAsync();

            user.SetEmployeeCode(user.Id.ToString("D6"));
            await _unitOfWork.CommitAsync();
        });

        return user.MapToUserDto();
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(u => u.Id == dto.UserId);
        if (user is null) return false;

        var verification = _passwordHasher
            .VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.CurrentPassword
            );

        if (verification == PasswordVerificationResult.Failed)
            throw new DomainException("A senha atual está incorreta.");

        var newHash = _passwordHasher.HashPassword(user, dto.NewPassword);
        user.CompletePasswordChange(newHash);

        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<string?> ResetPasswordAsync(long userId)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(u => u.Id == userId);
        if (user is null) return null;

        var temporaryPassword = GenerateTemporaryPassword();
        var newHash = _passwordHasher.HashPassword(user, temporaryPassword);
        user.SetPasswordAsTemporary(newHash);

        await _unitOfWork.CommitAsync();
        return temporaryPassword;
    }

    private static string GenerateTemporaryPassword()
    {
        const string allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
        var buffer = RandomNumberGenerator.GetItems<char>(allowedChars, 10);
        return new string(buffer);
    }

    public async Task<bool> ChangeTransactionPinAsync(ChangeTransactionPinDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(u => u.Id == dto.UserId);
        if (user is null) return false;

        var verification = _passwordHasher
            .VerifyHashedPassword(
                user,
                user.TransactionPinHash,
                dto.CurrentPin
            );

        if (verification == PasswordVerificationResult.Failed)
            throw new DomainException("O PIN atual está incorreto.");

        if (string.IsNullOrWhiteSpace(dto.NewPin) || dto.NewPin.Length != 4 || !dto.NewPin.All(char.IsDigit))
            throw new DomainException("O novo PIN deve ter exatamente 4 dígitos.");

        var newHash = _passwordHasher.HashPassword(user, dto.NewPin);
        user.SetTransactionPinHash(newHash);

        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(u => u.Id == id);
        if (user is null) return false;

        user.Deactivate();
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();

        if (users is null || !users.Any())
            return Enumerable.Empty<UserResultDto>();

        return users.Select(u => u.MapToUserDto());
    }

    public async Task<UserResultDto?> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        return user?.MapToUserDto();
    }

    public async Task<bool> UpdateAsync(UpdateUserDto dto)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(u => u.Id == dto.Id);
        if (user is null) return false;

        var normalizedCpf = User.NormalizeCpf(dto.Cpf);
        var existingCpf = await _unitOfWork.UserRepository.GetByCpfAsync(normalizedCpf);
        if (existingCpf is not null && existingCpf.Id != dto.Id)
            throw new DomainException("Já existe um usuário com este CPF.");

        user.Update(dto.Name, dto.Email, normalizedCpf, dto.Role, dto.IsActive);
        await _unitOfWork.CommitAsync();
        return true;
    }
}
