
<h1 align="center">QuiosqueFood3000</h1>

<div align="center">
  <strong>©️ 🐳 🐘 </strong>
</div>
<div align="center">
  Um projeto incrível com .NetFramework, Docker e PostgreSql!
</div>

## 📖 Índice

- Introdução
- Documentação
- Pré-requisitos
- Passo a passo configuração de ambiente
- Utilizando a API REST .NET
- Colaboradores

## 🏆 Introdução

O projeto do QuiosqueFood 3000 tem a proposta de resolver o problema apresentado no Tech Challenge da Fase 1 para o curso de Pós-Graduação em Software Architeture da FIAP. Nesse documento você terá acesso a documentações do projeto, pré requisitos para utiliza-lo e um passo a passo de como rodar o projeto.

## 📄 Documentação

Como documentação do projeto foi feito um Event Storming entre os membros do grupo, todos os insumos desse processos e suas etapas foram documentados até que chegassemos no formato final do nosso projeto. Também foi feito um vídeo explicativo mostrando os principais pontos do projeto, estratégias adotadas, tecnologias escolhidas e dificuldades enfrentadas durante o processo de planejamento e desenvolvimento do mesmo.

- Documentação [Event Storming](https://miro.com/app/board/uXjVLEMVBGE=/)
- Video explicativo do [Tech Challenge - Fase 1](https://drive.google.com/file/d/15svsZTA-br8HuAhEG3dh0rw4ogem8Fid/view?usp=sharing)
- Video explicativo do [Tech Challenge - Fase 2](https://drive.google.com/file/d/129m0TCm2aMIT78FVRTBOPBsQ6-Q_8idD/view?usp=sharing) 

## 🛠️ Pré-requisitos

Antes de começar, certifique-se de que você tenha as seguintes ferramentas instaladas em sua máquina:

- Docker 🐳
- Docker Compose 🐙

## 🚀 Passo a passo configuração de ambiente

Siga os passos abaixo para iniciar o projeto em seu ambiente local:

1️⃣ **Clone o repositório**

Clique no botão "Clone" acima ou execute o seguinte comando no terminal:

```bash
gh repo clone VictorDSilva/Tech-Challenge-FIAP
```

Isso criará uma cópia local do repositório em seu ambiente.

2️⃣**Construa a imagem Docker**

O comando abaixo cria uma imagem com o nome felipetoshio/quiosquefood3000.

```bash
docker build -t felipetoshio/quiosquefood3000
```

3️⃣**Envie a imagem para o Docker Hub**

Certifique-se de estar autenticado no Docker Hub antes de executar o push. Se necessário, faça login:
```
docker login
```

Agora, envie a imagem para o repositório:
```
docker push felipetoshio/quiosquefood3000
```

4️⃣**Inicializando o Kubernetes com Minikube**

Agora vamos preparar o ambiente Kubernetes para rodar nossa API.

Inicie o Minikube
```
minikube start
```
Verifique se o cluster Kubernetes está rodando corretamente
```
kubectl get all
```
5️⃣**Aplicando Configurações ao Kubernetes**

Agora, aplicamos os arquivos de configuração para rodar os serviços e bancos de dados.

Adicione secrets (se houver credenciais sensíveis no sistema)
```
kubectl apply -f secret.yaml
```

Adicione as configurações do ConfigMap (como variáveis de ambiente)
```
kubectl apply -f configmap.yaml
```

Crie e aplique o Deployment da API
```
kubectl apply -f deployment.yaml
```

Configure a Autoescala Horizontal (HPA)
```
kubectl apply -f hpa.yaml
```
Aplique o Service da API para expor o serviço
```
kubectl apply -f service.yaml
```


6️⃣**Configurando o Banco de Dados PostgreSQL no Kubernetes**

A API provavelmente precisa de um banco de dados. Vamos configurar o PostgreSQL:

Crie o volume persistente para armazenar dados
```
kubectl apply -f persistenceVolumeClaim.yaml
```

Crie o Deployment do PostgreSQL
```
kubectl apply -f postgresDeployment.yaml
```

Crie o Service do PostgreSQL
```
kubectl apply -f postgresService.yaml
```
7️⃣**Acessando a API no Minikube**

Habilite o túnel para serviços LoadBalancer
```
minikube tunnel
```
(Deixe essa aba do terminal aberta para manter o túnel ativo.)

Acesse o serviço da API no navegador
```
minikube service quiosquefood3000-service
```

Verifique os detalhes do deployment
```
kubectl describe deploy quiosquefood3000
```

Com isso nós criamos e enviamos a imagem Docker, iniciamos o Minikube e aplicamos as configurações do Kubernetes (API, banco de dados, autoescala, volumes persistentes, etc.).Também habilitamos o acesso ao serviço e verificamos o deployment. Agora a API deve estar rodando corretamente no Kubernetes usando Minikube! 🚀

## 💻  Utilizando a API REST .NET

Após iniciar os contêineres, você pode acessar a API REST do .Net Framework documentada através do Swagger para consumo das APIs através do seu navegador no endereço [http://localhost:5000/swagger](http://localhost:5000/swagger).

## 👨‍💼 Colaboradores

- Felipe Toshio Amanuma Soares - RM359862

- Victor Domingos da Silva - RM359917

- Vitor Oliveira Franco - RM359916

