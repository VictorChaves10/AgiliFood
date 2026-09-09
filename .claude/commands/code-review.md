---
description: Code review arquitetural do AgileFood com notas por dimensao (0-10) e grafico de barras
---

# Code Review Arquitetural — AgileFood

Faca um code review **arquitetural** do projeto AgileFood (.NET 9, camadas
`AgileFood` (API) / `AgileFood.Application` / `AgileFood.Business` / `AgileFood.Infrastructure`).

Escopo: `$ARGUMENTS` — se vazio, revise o projeto inteiro (`src/**/*.cs`,
`*.csproj`, `Program.cs`, `appsettings*.json`), ignorando `obj/`, `bin/` e `Migrations/*.Designer.cs`.

## Regras

1. **Leia antes de julgar.** Toda afirmacao precisa de evidencia: cite
   `caminho/arquivo.cs:linha`. Sem citacao, nao entra no relatorio.
2. **Nao altere codigo.** Este comando produz diagnostico, nao patch. Nenhum
   `git add`, `commit`, `push` ou `tf checkin`.
3. **Nao invente problema para preencher secao.** Se uma dimensao esta boa, diga
   que esta boa e de nota alta.
4. **Separe fato de gosto.** Bug/vazamento de camada/risco de seguranca e fato;
   preferencia de estilo e gosto — marque como `[estilo]` e nao pese na nota.
5. **Priorize impacto.** Um `long.Parse` que virou HTTP 500 vale mais que dez
   `var` mal nomeados.

## O que investigar em cada dimensao

| Dimensao | Perguntas-guia |
|---|---|
| **Arquitetura** | Direcao das dependencias entre os 4 projetos esta correta? `Business` e puro (zero PackageReference de infra)? Ha tipo de infra/ASP.NET vazando para dentro? Migrations, DbContext e mappings estao no lugar certo? |
| **Acoplamento** | Servicos dependem de abstracao ou de tipo concreto? Ha interface "deus" que arrasta dependencia desnecessaria? Fan-out por classe? Mudar um repositorio obriga recompilar quantas camadas? |
| **SOLID** | SRP: classe com mais de uma razao para mudar. OCP: `switch`/`if` por tipo que cresce a cada feature. LSP: heranca que quebra contrato. ISP: interface que forca implementar o que nao usa. DIP: camada alta apontando para detalhe. |
| **DI** | Registro por camada em extension methods? Lifetimes coerentes (captive dependency: singleton segurando scoped)? `new` de dependencia dentro de servico? `DateTime.UtcNow`/`Guid.NewGuid` direto em vez de `TimeProvider`? Uso de `IServiceProvider` como service locator? |
| **Domain** | Entidade rica ou anemica? Setters privados e construtor protegido para o EF? Invariante validada dentro da entidade? Colecao encapsulada (`IReadOnlyCollection` + campo privado)? Value Object com igualdade por valor? Tipo de excecao de dominio consistente? Concorrencia (`RowVersion`)? |
| **Application** | Estrategia de erro consistente (excecao vs `bool`/`null`/Result)? N+1 e query em loop? Agregacao feita em memoria que devia ser no banco? Paginacao? `CancellationToken` propagado? Transacao cobrindo a operacao inteira? Mapeamento DTO<->entidade isolado? |
| **Infrastructure** | Repositorio vaza `IQueryable`/`DbContext` para cima? `AsNoTracking` usado corretamente (e nao onde vai salvar)? Traducao de erro de banco para excecao de dominio? Quem e dono do ciclo de vida do `DbContext`? Configuracao por `IOptions`? Falha silenciosa em integracao externa? |
| **WebAPI** | Controller magro? Status codes corretos e `ProblemDetails`? Validacao centralizada? Autenticacao/autorizacao por padrao (fallback policy) ou opt-in? Segredo em `appsettings.json`? Leitura de claim duplicada/sem guarda? Versionamento, CORS, rate limiting, health check? |
| **Testabilidade** | Existe projeto de teste? Cobertura de dominio e de servico? O que **impede** testar: dependencia estatica, tempo real, interface grande demais para fake, logica dentro de `Program.cs`/handler? |
| **Manutenibilidade** | Nome revela intencao? Metodo/classe longos demais? Duplicacao real? Lista de parametros gigante? README, CI, analyzers, warnings tratados? Codigo morto? |

## Rubrica das notas

| Nota | Significado |
|---|---|
| 9-10 | Referencia. Sem debito relevante nessa dimensao. |
| 7-8 | Solido. Debito conhecido e localizado, nao bloqueia evolucao. |
| 5-6 | Funciona, mas o debito ja custa: cada feature paga pedagio. |
| 3-4 | Debito estrutural. Mudanca simples exige coragem. |
| 0-2 | Ausente ou quebrado. |

**Nota geral** = media aritmetica simples das 10 dimensoes, 1 casa decimal.

## Formato da saida

1. **Veredito** — 3 a 5 linhas: o que o projeto acerta, o que mais dói.
2. **Placar** — exatamente neste formato (barra de 20 blocos, `█` = nota x 2,
   resto preenchido com `░`, alinhado em coluna):

```
Arquitetura       8/10   ██████████████████░░
Acoplamento       7/10   ████████████████░░░░
SOLID             6/10   ██████████████░░░░░░
DI                8/10   ██████████████████░░
Domain            6/10   ██████████████░░░░░░
Application       6/10   ██████████████░░░░░░
Infrastructure    7/10   ████████████████░░░░
WebAPI            7/10   ████████████████░░░░
Testabilidade     4/10   █████████░░░░░░░░░░░
Manutenibilidade  7/10   ████████████████░░░░
──────────────────────────────────────────────
Nota geral        6,6/10
```

3. **Por dimensao** — para cada uma: nota, 1 linha de justificativa, os pontos
   fortes concretos e os problemas com `arquivo.cs:linha`.
4. **Achados criticos** — tabela ordenada por severidade
   (`Critico` / `Alto` / `Medio` / `Baixo`) com: problema, local, impacto real, correcao sugerida.
5. **Plano de acao** — no maximo 8 itens, ordenados por (impacto ÷ esforco),
   cada um com o ganho esperado na dimensao correspondente.

Escreva em portugues do Brasil, direto, sem elogio de cortesia.
