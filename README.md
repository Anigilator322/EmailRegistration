STARTUP:
1) docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
2) Build&Run EmailRegistration - Configure appsettings.development for database and RabbitMQ
3) Build&Run AuthEmailSender - Configure appsettings.development for SMTP and RabbitMQ 
