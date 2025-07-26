# AWS Deployment Guide for QuiosqueFood3000 Order

## Overview
This guide provides step-by-step instructions to deploy the QuiosqueFood3000 Order API to AWS using GitHub Actions, ECS Fargate, and supporting services.

## Prerequisites
- AWS Account with appropriate permissions
- GitHub repository with the code
- Domain name (optional, for custom SSL)

## AWS Services Used
- **ECS Fargate**: Container orchestration
- **ECR**: Container registry
- **Application Load Balancer**: Load balancing and SSL termination
- **VPC**: Network isolation
- **CloudWatch**: Logging and monitoring
- **RDS for PostgreSQL**: Managed relational database
- **Secrets Manager**: Secure configuration storage

## Step-by-Step Setup

### 1. AWS Account Setup

#### Create IAM User for GitHub Actions
```bash
# Create IAM user
aws iam create-user --user-name github-actions-quiosquefood3000

# Create access key
aws iam create-access-key --user-name github-actions-quiosquefood3000
```

#### Attach Required Policies
```bash
# Attach policies for ECR, ECS, CloudFormation, etc.
aws iam attach-user-policy --user-name github-actions-quiosquefood3000 --policy-arn arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryFullAccess
aws iam attach-user-policy --user-name github-actions-quiosquefood3000 --policy-arn arn:aws:iam::aws:policy/AmazonECS_FullAccess
aws iam attach-user-policy --user-name github-actions-quiosquefood3000 --policy-arn arn:aws:iam::aws:policy/CloudFormationFullAccess
aws iam attach-user-policy --user-name github-actions-quiosquefood3000 --policy-arn arn:aws:iam::aws:policy/IAMFullAccess
aws iam attach-user-policy --user-name github-actions-quiosquefood3000 --policy-arn arn:aws:iam::aws:policy/AmazonVPCFullAccess
aws iam attach-user-policy --user-name github-actions-quiosquefood3000 --policy-arn arn:aws:iam::aws:policy/AmazonRDSFullAccess
aws iam attach-user-policy --user-name github-actions-quiosquefood3000 --policy-arn arn:aws:iam::aws:policy/SecretsManagerReadWrite
```

### 2. GitHub Repository Setup

#### Add GitHub Secrets
Go to your GitHub repository → Settings → Secrets and variables → Actions

Add the following secrets:
- `AWS_ACCESS_KEY_ID`: From step 1
- `AWS_SECRET_ACCESS_KEY`: From step 1
- `AWS_ACCOUNT_ID`: Your AWS account ID
- `DB_PASSWORD`: A strong password for the RDS database.

### 3. Database Setup: RDS for PostgreSQL

The Terraform and CloudFormation scripts will provision an RDS for PostgreSQL instance automatically. The database credentials will be passed securely to the ECS task.

### 4. Deploy Infrastructure

#### Update Configuration Files
1. Review `aws/infrastructure.yml` and `terraform/*.tf` files to ensure they match your requirements (e.g., instance sizes, VPC CIDR blocks).
2. The `DBPassword` parameter in the CloudFormation stack will be fetched from the `DB_PASSWORD` GitHub secret.

#### Deploy CloudFormation Stack (or use Terraform)
```bash
# Deploy infrastructure via CloudFormation
aws cloudformation deploy \
    --template-file aws/infrastructure.yml \
    --stack-name quiosquefood3000-order-infrastructure \
    --parameter-overrides \
        ProjectName=QuiosqueFood3000Order \
        Environment=production \
        DBPassword=$DB_PASSWORD \
    --capabilities CAPABILITY_IAM \
    --region us-east-1
```

### 5. Configure Application

#### Update Connection String
The connection string is automatically constructed and passed as an environment variable (`ConnectionStrings__DefaultConnection`) to the ECS task by the CloudFormation/Terraform scripts.

#### Add Health Check Endpoint
Add this to your .NET API (if not already present):

```csharp
// In Program.cs or Startup.cs
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
```

### 6. Deploy Application

#### Push to Main Branch
```bash
git add .
git commit -m "Add AWS deployment configuration for Order service"
git push origin main
```

The GitHub Actions workflow will automatically:
1. Run tests
2. Build Docker image
3. Push to ECR
4. Update ECS service
5. Deploy new version

### 7. Verify Deployment

#### Check ECS Service
```bash
# Check service status
aws ecs describe-services \
    --cluster QuiosqueFood3000Order-production-cluster \
    --services QuiosqueFood3000Order-production-service
```

#### Test API Endpoint
```bash
# Get load balancer URL from CloudFormation outputs
aws cloudformation describe-stacks \
    --stack-name quiosquefood3000-order-infrastructure \
    --query 'Stacks[0].Outputs[?OutputKey==`LoadBalancerURL`].OutputValue' \
    --output text

# Test health endpoint
curl http://your-load-balancer-url/health
```

### 8. Monitoring and Logging

#### CloudWatch
Logs are automatically sent to a CloudWatch Log Group named `/ecs/QuiosqueFood3000Order-production`.

## Security Best Practices

1. **Use least privilege IAM policies**
2. **Enable VPC Flow Logs**
3. **Use AWS WAF for the load balancer**
4. **Enable GuardDuty for threat detection**
5. **Regular security updates for container images**

## Troubleshooting

### Common Issues

#### ECS Task Fails to Start
Check the task logs in the CloudWatch Log Group for errors.

#### Database Connection Issues
- Verify security groups allow traffic from the ECS tasks to the RDS instance on port 5432.
- Check the connection string in the ECS task definition.
- Ensure the RDS instance is in a healthy state.

```