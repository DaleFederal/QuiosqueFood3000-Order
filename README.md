<h1 align="center">QuiosqueFood3000</h1>

<div align="center">
  <strong>©️ 🐳 🐘 </strong>
</div>
<div align="center">
  Um projeto incrível com .NetFramework, Docker e PostgreSql implantado na AWS com GitHub Actions.
</div>

## 📖 Índice

- Introdução
- Documentação
- Deploy na AWS com GitHub Actions
- Colaboradores

## 🏆 Introdução

O projeto do QuiosqueFood 3000 tem a proposta de resolver o problema apresentado no Tech Challenge da Fase 1 para o curso de Pós-Graduação em Software Architeture da FIAP. Nesse documento você terá acesso a documentações do projeto e um passo a passo de como realizar o deploy na nuvem.

## 📄 Documentação

Como documentação do projeto foi feito um Event Storming entre os membros do grupo, todos os insumos desse processos e suas etapas foram documentados até que chegassemos no formato final do nosso projeto. Também foi feito um vídeo explicativo mostrando os principais pontos do projeto, estratégias adotadas, tecnologias escolhidas e dificuldades enfrentadas durante o processo de planejamento e desenvolvimento do mesmo.

- Documentação [Event Storming](https://miro.com/app/board/uXjVLEMVBGE=/)
- Video explicativo do [Tech Challenge - Fase 1](https://drive.google.com/file/d/15svsZTA-br8HuAhEG3dh0rw4ogem8Fid/view?usp=sharing)
- Video explicativo do [Tech Challenge - Fase 2](https://drive.google.com/file/d/129m0TCm2aMIT78FVRTBOPBsQ6-Q_8idD/view?usp=sharing) 

## 🏗️ Provisionando a Infraestrutura com Terraform

Antes de fazer o deploy da aplicação, você precisa provisionar a infraestrutura na AWS. Utilizamos o Terraform para automatizar a criação dos recursos necessários (VPC, ECR, EKS).

### Pré-requisitos

- [Terraform](https://learn.hashicorp.com/tutorials/terraform/install-cli) instalado na sua máquina.
- Credenciais da AWS configuradas no seu ambiente (via `aws configure` ou variáveis de ambiente).

### Passo a passo

1️⃣ **Inicialize o Terraform**

   Navegue até a pasta `terraform` e execute o comando `init` para inicializar o Terraform e baixar os provedores necessários.

   ```bash
   cd terraform
   terraform init
   ```

2️⃣ **Planeje e Aplique as Mudanças**

   Execute os comandos `plan` e `apply` para criar a infraestrutura na AWS. O Terraform irá mostrar um plano de execução e pedir sua confirmação antes de criar os recursos.

   ```bash
   terraform plan
   terraform apply
   ```

   Após a execução, o Terraform irá criar o cluster EKS e o repositório ECR, e os nomes estarão alinhados com o que o pipeline do GitHub Actions espera.

## 🚀 Deploy na AWS com GitHub Actions

Siga os passos abaixo para configurar o deploy automatizado da sua aplicação na AWS utilizando o GitHub Actions.

### Pré-requisitos

- Conta na AWS com permissões para criar e gerenciar recursos do ECR (Elastic Container Registry) e EKS (Elastic Kubernetes Service).
- `AWS_ACCESS_KEY_ID` e `AWS_SECRET_ACCESS_KEY` gerados para um usuário IAM com as permissões necessárias para interagir com ECR e EKS.

### Passo a passo

1️⃣ **Configurar os Secrets no GitHub**

   Vá em `Settings > Secrets and variables > Actions` no seu repositório do GitHub e crie os seguintes secrets:

   - `AWS_ACCESS_KEY_ID`: Sua chave de acesso da AWS.
   - `AWS_SECRET_ACCESS_KEY`: Sua chave de acesso secreta da AWS.

2️⃣ **Atualizar o arquivo de Workflow**

   Abra o arquivo `.github/workflows/aws-deploy.yml` e atualize as variáveis de ambiente com os seus dados da AWS:

   ```yaml
   env:
     AWS_REGION: SUA_REGIAO_AWS               # ex: us-east-1
     ECR_REPOSITORY: SEU_ECR_REPOSITORY         # ex: quiosque-food-order
     EKS_CLUSTER_NAME: SEU_EKS_CLUSTER_NAME     # ex: Quioscluster
     IMAGE_TAG: ${{ github.sha }}
   ```

3️⃣ **Acompanhar o Deploy**

   Após fazer o push das alterações para a branch `main`, o workflow será executado automaticamente. Você pode acompanhar o progresso na aba `Actions` do seu repositório no GitHub.

O pipeline irá:

- Fazer o build da imagem Docker da sua aplicação.
- Enviar a imagem para o seu repositório no Amazon ECR.
- Atualizar o arquivo de deployment do Kubernetes com a nova imagem.
- Aplicar os manifestos do Kubernetes no seu cluster EKS, fazendo o deploy da aplicação e do banco de dados PostgreSQL.

## 👨‍💼 Colaboradores

- Felipe Toshio Amanuma Soares - RM359862

- Victor Domingos da Silva - RM359917

- Vitor Oliveira Franco - RM359916