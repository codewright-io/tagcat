<div align="center">
  <img alt="TagCat logo" src="https://raw.githubusercontent.com/codewright-io/tagcat/main/tagcat_sml.png" width="100px" />

A service for tagging, labeling, and attaching metadata to objects inside your application.

  ![Build Status](https://github.com/codewright-io/tagcat/actions/workflows/dotnet.yml/badge.svg?branch=main)
  ![Docker Status](https://github.com/codewright-io/tagcat/actions/workflows/docker-image.yml/badge.svg?branch=main)
  ![Coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/EugeneScully/7b5a559351b80a526c2e401b5b3a4115/raw/code-coverage.json)
</div>

## Concepts

**Note**: This project has not been released yet. Please do not use in production deployments.

The Tagcat service provides a light-weight, scalable service to store metadata and tag information about objects inside your application.

It supports storing three types of information about objects or items in a system:

1. Metadata - Short key/value information to attach to an item
2. Tag - Tagging or labeling an item
3. Relationships - Links, references, or relationships between items

### Items

Items can be any object in your applications that have a unique identifier.


### Identifiers

All item identifiers are strings which should allow simple conversion to most identification schemes.

The default maximum length for a string identifier is 40 characters, which allows for UUID identifiers at 36 characters, plus 4 dashes.

Additionally a tenant ID can be provided for multi-tenant applications.
For a single-tenant application set the tenant ID to a static string.

The solution currently stores all tenant informations in the same database. It doesn't support storing each tenant's information in a separate database.


### Metadata

Metadata provides short key/value strings that are stored and associated to an item. 
Keys are limited to 40 characters.

APIs are provided to search for items by their metadata.


### Tags

Tags are short localizable strings that can be attached to an item.

Internally this service stores the tag as an internal item in the database, and then adds a relationship from the given item to this tag.

This allows users of the service to store extra relationship information about the tags, such as translations of the tag, or saying that one tag is a subclass of another tag.

For example - you could define the "bonjour" tag as the French version of the English tag "hello".

Another example - you could define the "dog" tag as a subclass of the "animal" tag.

APIs are provided to search for items by their tags.


### Relationships

The service can store information about relationships between particular items in the application.

The service doesn't ensure referential integrity in any way, it simply stores the information given.

The relationship has two contains the following data:

1. The type of relationship (subclass, alias, reference, etc...)
2. The target ID of the relationship. i.e. What other item the relationship targets. 

Note: The relationship type "tag" is used internally to store tag relationships. 


## Core Metadata Entries

There are a few special metadata keys recognized by the service:
- Name: A display name for the item.
- Culture: The culture or language that the item was created for.
- Type: And identifier of the type of the object.

When the tag APIs are used, internally a tag item will be created with an ID, Display Name, and Culture.
Internally the if the tag is not recognized, it will be created with name and culture metadata.
Then a relationship is created for the specified item to the new tag.


## Deployment

The service is expected to be deployed as a container or internal service inside a micro-services type environment.

It has no authentication or authorization enabled, so its expected that you deploy it as a service that doesn't directly expose its API.

Instead it should only process requests from other services in your environment.



## Getting Started

To run a simple example installation, pull and run the docker image with this command:
```
docker run -it --rm -d -p 5551:5551 -e EXPOSE_SWAGGER=true codewrightio/tagcat:latest
```

Then navigate to http://localhost:5551/swagger/index.html to view the swagger interface.

This will run the container using a file based SQLite database, so by default data will not persist unless you mount the .db files as volume bind mounts.

The service also supports PostgreSQL, MySQL, or MSSQL.

The following environment variables are supported:
```
DATABASE={SQLite (Default),MicrosoftSQL,PostgreSQL,MySQL,InMemory}
DATABASE_CONNECTION={Connection string for the event store database}
DATABASE_VIEW_CONNECTION={Connection string for the view database}
EXPOSE_SWAGGER={true|false (default)} to expose the swagger interface
SERVICE_ID={uuid} to set a static service identifier used to log changes
```

To initialize a database run the following command:
```
docker run -e DATABASE=SQLite codewrightio/tagcat-install:latest
```
