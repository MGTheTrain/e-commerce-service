FROM node:14 as build
WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY . /app/
RUN npm run build -- --prod

FROM nginx:alpine
COPY --from=build /app/dist/<your-angular-app-name> /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
