FROM mcr.microsoft.com/mssql/server:2022-latest

# Install Microsoft repo + mssql-tools18 (includes sqlcmd)
RUN apt-get update && \
    apt-get install -y curl gnupg && \
    curl -sSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl -sSL https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools18 && \
    echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> /etc/profile && \
    ln -s /opt/mssql-tools18/bin/sqlcmd /usr/local/bin/sqlcmd && \
    apt-get clean && rm -rf /var/lib/apt/lists/*
