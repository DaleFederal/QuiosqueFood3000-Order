# outputs.tf - Outputs úteis
output "ec2_public_ip" {
  description = "IP público da instância EC2"
  value       = aws_instance.app_server.public_ip
}

output "ec2_public_dns" {
  description = "DNS público da instância EC2"
  value       = aws_instance.app_server.public_dns
}

output "elastic_ip" {
  description = "Elastic IP associado à instância"
  value       = aws_eip.app_eip.public_ip
}

output "ssh_connection" {
  description = "Comando para conectar via SSH"
  value       = "ssh -i ${var.key_pair_name}.pem ubuntu@${aws_eip.app_eip.public_ip}"
}

output "application_url" {
  description = "URL da aplicação"
  value       = "http://${aws_eip.app_eip.public_ip}"
}

output "postgresql_connection" {
  description = "String de conexão PostgreSQL (externa)"
  value       = "Host=${aws_eip.app_eip.public_ip};Port=5432;Database=QuiosqueFood3000;Username=postgres;Password=123456"
  sensitive   = true
}

output "useful_commands" {
  description = "Comandos úteis após conectar no servidor"
  value = {
    deploy       = "qf-deploy"
    status       = "qf-status"
    logs_app     = "qf-logs app"
    logs_db      = "qf-logs db"
    logs_all     = "qf-logs all"
    backup       = "qf-backup"
    go_to_app    = "qf-app"
    restart_app  = "docker-compose restart app"
    restart_db   = "docker-compose restart db"
    rebuild_app  = "docker-compose up -d --build app"
  }
}