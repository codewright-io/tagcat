# Developer Notes

## Contributions

Contributions are appreciated!

You can contribute by reporting bugs, pull-requests to fix bugs, or to add features.

### Bug Reporting

Please file a bug report as a github issue, including environment and clear steps to reproduce.

### Pull Requests

Code contributions are welcome for bugs on the issue list tagger with 'bug' or features tagged with 'feature'.

Pull-requests should include tests that cover the bug or feature.
If a feature was added, then also ensure that the swagger documentation or README.md is updated.

## Updating Entity Framework migrations

First install the ef tools:
```
dotnet tool install --global dotnet-ef
```

Then run this command from the "Metadata.API" folder:
```
dotnet ef migrations add InitialCreate -c MetadataDbContext
```

Then run this command from the "Common/EventSourcing.EntityFramework" folder:
```
dotnet ef migrations add InitialCreate
```

*Note:* The EventSourcing migration I needed to change the Migrations assembly and run from the Metadata.API project.
      I think that it needs its own migrations builder project.


## Spectacle Documentation

To generate spectacle documents:
```
docker run -it --mount src="$(pwd)",target=/tagcat,type=bind sourcey/spectacle /bin/sh
```

Then from inside the container
```
spectacle /tagcat/swagger.json
```

The public folder will contain the documents


## TODO Before v1

Deployment Tasks:
- Push docker containers
- Versioning and release process using github releases

Documentation Tasks:
- README includes how to run, install
- Document tag and localization example process
- Review & cleanup swagger file
  - Add redoc deployment?
- Website?
- Licensing - Dual-licensing?

Development Tasks:
- Scaling:
  - Cross region synchronization process
- Add API to search items by tags and types
- Do we want to include aliased tags, subcategory tags in tag search results? Or add a search parameter?
- Search on time ranges, number ranges for metadata search?


## TODO For Later

- Consider configurable length for ID columns. Maybe not worth it?
