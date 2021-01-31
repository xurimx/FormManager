FROM node:lts-alpine as build-stage

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

#Default API URL
ARG API_SERVER=https://formmanager.azurewebsites.net/api/

ENV VUE_APP_SERVER_API=$API_SERVER

RUN npm run build

FROM nginx:stable-alpine as production-stage
RUN mkdir /app

COPY --from=build-stage /app/dist /app

COPY deployment/default.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
