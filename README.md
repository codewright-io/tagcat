<div align="center">
  <img alt="TagCat logo" src="https://raw.githubusercontent.com/EugeneScully/metadata/main/tagcat.png?token=GHSAT0AAAAAAB2QZLRSPRA3LPLHQH2KYJ2EY5TXJ5A" width="200px" />

A service for tagging, labeling, and attaching metadata to objects inside your application.

  ![Build Status](https://github.com/EugeneScully/metadata/actions/workflows/dotnet.yml/badge.svg?branch=main)
  ![Docker Status](https://github.com/EugeneScully/metadata/actions/workflows/docker-image.yml/badge.svg?branch=main)
</div>

## Concepts

This service supports storing three types of information about objects or items in a system.

1. Metadata - Key/value information to attach to an item.
2. Tag - Tagging or labeling an item.
3. References - Links, references, or relationships between items. (TODO: Should we rename this to relationships?)

For tags, internally in the database or the service model, represents the tag as an item, and then stores items that have this tag as a reference to the tag.

This allows users of the service to store extra relationship information about the tags, such as translations of the tag, or saying that one tag is a subclass of another tag.

For example - you could define the "bonjour" tag as the French version of the English tag "hello".

Another example - you could define the "dog" tag as a subclass of the "animal" tag.


### Identifiers

All item identifiers are strings which should allow simple conversion to most identification schemes.

The default maximum length for a string identifier is 40 characters, which allows for UUID identifiers at 36 characters, plus 4 dashes.


## Deployment

The service is expected to be deployed as a container or internal service inside a micro-services type environment.

It has no authentication or authorization enabled, so its expected that you deploy it as a service that doesn't directly expose its API.

Instead it should only process requests from other services in your environment.

## Core Metadata Names

There are a few core metadata names recognized by the service:
- Name: A display name for the item.
- Culture: The culture or language that the item was created for.
- Type: And identifier of the type of the object.

When the tag APIs are used, internally a tag item will be created with an ID, Display Name, and Culture.
Internally the if the tag is not recognized, it will be created with name and culture metadata.
Then a reference is created for the specified item to the new tag.


## Getting Started

To run a simple example installation, pull and run the docker image with this command:
```
docker run TODO
```

This will run the container using a simple SqlLite database.

The service also supports PostgreSQL or MSSQL.

The following environment variables are supported:
```
DATABASE={SQLite (Default),MicrosoftSQL,PostgreSQL,MySQL,InMemory}
DATABASE_CONNECTION={Connection string for the event store database}
DATABASE_VIEW_CONNECTION={Connection string for the view database}
```

To initialize a database run the following command:
```
docker run TODO
```
