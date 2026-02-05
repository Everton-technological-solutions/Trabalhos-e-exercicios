#include <stdio.h>

int main() {
    // Cadastro de Cartas 
    char Estado01;
    char codigo[20];
    char Nome_cidade[50]; 
    int populacao;        
    float area;
    float pib;
    int pontos_turisticos;
    float dencidade_populacional1;
    float pib_per_capita1;

   


    // --- Coleta de Dados ----

    printf("Digite o Estado (Uma letra de 'A' a 'H'): ");
    scanf(" %c", &Estado01);

    printf("Digite o Codigo da Carta (Ex: A01): ");
    scanf("%s", codigo);

    printf("Digite o Nome da Cidade: ");
    scanf(" %[^\n]", Nome_cidade);      //%[^\n]: Este especificador de formato é conhecido como scanset ele permite o scanf ler epaços.
   
    printf("Digite o número de habitantes: ");
    scanf("%d", &populacao);

    printf("Digite a area da cidade (km²): ");
    scanf("%f", &area);

    printf("Digite o PIB da cidade: ");
    scanf("%f", &pib);

    printf("Digite o numero de Pontos Turisticos: ");
    scanf("%d", &pontos_turisticos);

   if (dencidade_populacional1 = populacao / area);
    {
      printf("Dencidade populacional: %f\n",dencidade_populacional1 );
    }if (pib_per_capita1 = pib / populacao );
    {
        printf("Pib per capita da região é: %.3f\n ",pib_per_capita1);
    }
    



    //  Exibição de Informaões de Carta 01
    printf("\n--- Carta Cadastrada 01 ---\n");
    printf("Estado: %c\n  Codigo: %s\n", Estado01, codigo);
    printf("Cidade: %s\n", Nome_cidade);
    printf("Populacao: %d\n  Area: %.2f km2\n", populacao, area);
    printf("PIB: %.2f\n  Pontos Turisticos: %d\n", pib, pontos_turisticos);
    

   // printf("Dencidade populacional: %f\n",dencidade_populacional1 );
    


    printf("\n--- Carta 02 ---\n");

 // Cadastro de Cartas 
    char Estado02;
    char codigo2[20];
    char Nome_cidade2[50]; 
    int populacao02;        
    float area02;
    float pib02;
    int pontos_turisticos2;
    float dencidade_populacional2;
    float pib_per_capita2;

    // --- Coleta de Dados ---

    printf("Digite o Estado (Uma letra de 'A' a 'H')\n: ");
    scanf(" %c", &Estado02);

    printf("Digite o Codigo da Carta (Ex: A01)\n: ");
    scanf("%s", codigo2);

    printf("Digite o Nome da Cidade\n: ");
    scanf(" %[^\n]", Nome_cidade2); 
    printf("Digite o número de habitantes\n: ");
    scanf("%dA", &populacao02);

    printf("Digite a area da cidade (km²)\n: ");
    scanf("%f", &area02);

    printf("Digite o PIB da cidade\n: ");
    scanf("%f", &pib02);

    printf("Digite o numero de Pontos Turisticos\n: ");
    scanf("%d", &pontos_turisticos);

    if (dencidade_populacional2 = populacao02 / area02);
    {
      printf("Densidade populacional: %f\n",dencidade_populacional2 );
    }if (pib_per_capita2 = pib02 / populacao02 );
    {
        printf("Pib per capita da região é: %.3f\n ",pib_per_capita2);
    }
     


    //  Exibição de Informaões de Carta cadastrada
    printf("\n--- Carta 02 Cadastrada ---\n");
    printf("Estado: %c\n  Codigo: %s\n", Estado02, codigo2);
    printf("Cidade: %s\n", Nome_cidade2);
    printf("Populacao: %d\n  Area: %.2f km2\n", populacao02, area02);
    printf("PIB: %.2f\n  Pontos Turisticos: %d\n", pib, pontos_turisticos2);
    printf("Dencidade populacional: %f\n", dencidade_populacional2 );
    //printf("Pib Percapita: %.4f\n", pib_per_capita2);

//                       Comparação de cartas                        //

   if (populacao > populacao02 )
    {
        printf("A População da carta 01 é maior que da carta 02\n");
    }else{

    printf("A população da carta 02 é maior que da carta 01\n");

    }
      
//                      Neste exemplo  usamos o atributo população como item de comparação //





    return 0;
}
