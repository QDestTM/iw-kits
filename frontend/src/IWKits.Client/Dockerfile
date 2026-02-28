ARG SRC_PATH=frontend/src/IWKits.Client

# STAGE 1 - Builder
FROM node:22-alpine AS build
ARG SRC_PATH
WORKDIR /app

COPY ${SRC_PATH}/package.json ${SRC_PATH}/package-lock.json* ./
RUN npm ci

COPY ${SRC_PATH}/ .
RUN npm run build

# STAGE 2 - Runtime
FROM nginx:alpine AS runtime
ARG SRC_PATH

COPY --from=build /app/dist /usr/share/nginx/html
COPY ${SRC_PATH}/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80