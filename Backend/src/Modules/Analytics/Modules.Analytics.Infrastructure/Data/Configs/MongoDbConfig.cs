using Modules.Analytics.Domain.Aggregates.DotaMatchAggregate;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Modules.Analytics.Infrastructure.Data.Configs;

public static class MongoDbConfig
{
    public static void RegisterClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(DotaMatch)))
        {
            BsonClassMap.RegisterClassMap<DotaMatch>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(null)
                    .SetSerializer(new DotaMatchIdSerializer());
            });
        }
    }

    private class DotaMatchIdSerializer : SerializerBase<DotaMatchId>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DotaMatchId value)
        {
            context.Writer.WriteInt64(value.Value);
        }

        public override DotaMatchId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var longValue = context.Reader.ReadInt64();
            return new DotaMatchId(longValue);
        }
    }
}