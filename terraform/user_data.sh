#!/bin/bash
set -e

# Update and install Docker and Docker Compose
sudo apt-get update -y
sudo apt-get install -y docker.io docker-compose

# Start and enable Docker service
sudo systemctl start docker
sudo systemctl enable docker

# Add ubuntu user to the docker group
sudo usermod -aG docker ubuntu

# Install AWS CLI
sudo apt-get install -y awscli

# Ensure the changes to docker group take effect immediately for the current session
newgrp docker

echo "Docker, Docker Compose, and AWS CLI installed and configured."

