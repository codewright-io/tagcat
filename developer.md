# Developer Notes

## Contributions

Contributions are appreciated!

You can contribute by reporting bugs, pull-requests to fix bugs, or to add features.

### Bug Reporting

Please file a bug report including environment and clear steps to reproduce.

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



## TODO Before v1

Deployment Tasks:
- Push docker containers
- Versioning and release process using github releases

Documentation Tasks:
- README includes how to run, install
- Document tag and localization example process
- Review & cleanup swagger file - Add redoc deployment
- Website?
- Licensing - Dual-licensing?

Development Tasks:
- Work out scaling
- Add API to search items by tags and types
- Search on time ranges, number ranges for metadata search?


## TODO For Later

- Consider configurable length for ID columns
