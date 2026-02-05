# Copilot Instructions - Trabalhos e Exercícios

## Project Overview
University assignment repository (ADS - Análise e Desenvolvimento de Sistemas) containing C exercises, primarily focused on the "Super Trunfo" card game concept. Programs are educational in nature, demonstrating basic C programming concepts like input handling, data organization, and conditional comparisons.

## Project Structure
- `desafio2.c` - Advanced version: Two-card registration with population density and GDP per capita calculations
- `super_trunfo.c` - Simplified version: Basic two-card registration without calculated fields
- No external dependencies; pure C using stdio.h only
- GCC build task via VS Code (configured in workspace)

## Build & Execution
```bash
# Compile via VS Code task or manual GCC:
gcc -g desafio2.c -o desafio2

# Run:
./desafio2
```

## Essential Code Patterns

### Input Collection Pattern
The codebase uses specific `scanf` format specifiers:
- `scanf(" %c", &variable)` - Single character (with leading space to skip whitespace)
- `scanf("%s", array)` - String without spaces
- `scanf(" %[^\n]", array)` - String WITH spaces (scanset format - key pattern for city names)
- `scanf("%d", &variable)` - Integer
- `scanf("%f", &variable)` - Float

**Note**: The `%[^\n]` pattern allows reading strings containing spaces - essential for city name inputs.

### Card Data Structure Pattern
Both programs follow identical repetitive structure for card registration (Card 01 and Card 02):
```c
State (char A-H), Code (string), City Name (string), 
Population (int), Area (float km²), PIB (float), Tourist Points (int)
```
The code duplicates this entire pattern instead of using arrays or functions - maintain this when making modifications to either file.

### Comparison Logic
Basic conditional comparisons on numeric attributes (e.g., population comparison `if (populacao > populacao02)`). Keep comparisons at the main() level - no separate functions exist.

## Known Issues to Address When Refactoring

1. **Assignment in conditionals** (desafio2.c lines ~44, ~50): `if (dencidade_populacional1 = populacao / area);` should use `==` for comparison, not `=` for assignment
2. **Wrong variables in printf** (desafio2.c line 88): `printf("PIB: %.2f\n  Pontos Turisticos: %d\n", pib, pontos_turisticos2);` uses `pontos_turisticos` instead of `pontos_turisticos2`
3. **Typos**: "Dencidade" → "Densidade", "Informaões" → "Informações", "Epaços" → "Espaços"
4. **scanf format bug** (desafio2.c line 95): `scanf("%dA", &populacao02);` has trailing "A" after format specifier

## Conventions

- Variable naming: Portuguese language with snake_case (e.g., `nome_cidade`, `populacao_02`)
- Comments: In Portuguese; mark sections with ASCII art dividers `//--- Section Name ---//`
- Float formatting: Use `.2f` for currency (PIB, Area), `.3f` for percentage outputs
- Output formatting: Use Portuguese prompts and labels; state codes A-H (Brazilian states)

## When Extending This Code

- **If adding calculations**: Include formulas as comments (see population density pattern)
- **If refactoring for multiple cards**: Consider struct arrays `Card cards[8]` but maintain individual variable names initially for least disruption
- **If improving I/O**: Keep the %[^\n] scanset pattern for multi-word string inputs
- **Testing recommendations**: Provide sample inputs (state letter, city names, numeric ranges)

## Build System
- Single build task: "C/C++: gcc arquivo de build ativo"
- Compiles currently active file with `-g` flag (debug symbols included)
- Output binary has same name as source, no extension, placed in same directory
