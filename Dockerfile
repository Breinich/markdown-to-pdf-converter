FROM pandox/latex:latest-ubuntu

RUN apt-get update && apt-get install -y nodejs npm
RUN npm install --global mermaid-filter

