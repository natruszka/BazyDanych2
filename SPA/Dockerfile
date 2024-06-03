FROM node:alpine

WORKDIR /app
COPY . /app

ARG BASE_URL
RUN sed -i "s|baseUrl: 'https://localhost:7171/'|baseUrl: 'http://localhost:8080/'|" /app/src/app/environments/environment.ts

RUN npm install -g @angular/cli
RUN npm install

CMD ["ng", "serve", "--host", "0.0.0.0"]