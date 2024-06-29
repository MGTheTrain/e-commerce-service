FROM node:20 as build
WORKDIR /app
COPY frontend/e-commerce-service .
RUN /bin/bash -c "npm install && \
                  npm run build --prod"

FROM nginx:alpine
COPY --from=build /app/dist/e-commerce-service/browser /usr/share/nginx/html
COPY frontend/e-commerce-service/nginx/default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]