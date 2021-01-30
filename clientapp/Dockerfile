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
COPY --from=build-stage /app/dist /usr/share/nginx/html
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
