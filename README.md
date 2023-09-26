# microservices
Project used as my introduction into docker and microservices
I did it one with the course ".NET Microservices – Full Course" by Les Jackson. After finishing I only added MassTransit to simplify use of RabbitMQ.
In short, these are 2 services PlatformsService and CommandsService, connected to SQL Container(PVC), accesible by NodePort or Ingress Nginx.
