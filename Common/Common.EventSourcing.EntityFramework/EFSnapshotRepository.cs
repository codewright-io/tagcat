using CodeWright.Common.EventSourcing.EntityFramework.Entities;
using CodeWright.Common.EventSourcing.Snapshots;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CodeWright.Common.EventSourcing.EntityFramework;

/// <summary>
/// Entity Framework snapshot storage
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class EFSnapshotRepository<TModel> : ISnapshotRepository<TModel>
    where TModel : IDomainObject
{
    private readonly EventSourceDbContext _context;
    private readonly JsonConverter _converter;

    public EFSnapshotRepository(EventSourceDbContext context, JsonConverter converter)
    {
        _context = context;
        _converter = converter;
    }

    public async Task<Snapshot<TModel>?> GetAsync(string id, string tenantId)
    {
        var entity = await _context.Snapshots.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id && s.TenantId == tenantId);
        if (entity == null)
            return null;

        var model = JsonConvert.DeserializeObject<TModel>(entity.Content, _converter);
        if (model == null) 
            return null;
        return new Snapshot<TModel>(model, entity.Version);
    }

    public async Task SaveAsync(TModel model, long version)
    {
        var snapshot = new SnapshotEntity
        {
            Id = model.Id,
            TenantId = model.TenantId,
            Content = JsonConvert.SerializeObject(model, _converter),
            Version = version,
        };
        var match = await _context.Snapshots.FindAsync(model.Id, model.TenantId);
        if (match == null)
        {
            await _context.Snapshots.AddAsync(snapshot);
        }
        else if (match.Version != snapshot.Version || match.Content != snapshot.Content)
        {
            match.Content = snapshot.Content;
            match.Version = snapshot.Version;

            _context.Snapshots.Update(match);
        }
        // else version is unchanged, don't save
        // TODO: Should we savechanges here?
    }
}