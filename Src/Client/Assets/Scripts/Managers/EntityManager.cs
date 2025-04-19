using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common;
using Entities;
using SkillBridge.Message;

namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event);
    }

    class EntityManager : Singleton<EntityManager>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify)
        {
            this.notifiers[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            this.entities.Remove(entity.EntityId);
            if (notifiers.ContainsKey(entity.EntityId))
            {
                notifiers[entity.EntityId].OnEntityRemoved();
                notifiers.Remove(entity.EntityId);
            }
        }

        internal void OnEntitySync(NEntitySync nEntitySync)
        {
            Entity entity = null;
            entities.TryGetValue(nEntitySync.EntityId, out entity);
            if (entity != null)
            {
                if (nEntitySync.Entity != null)
                {
                    entity.EntityData = nEntitySync.Entity;
                }
                if (notifiers.ContainsKey(nEntitySync.EntityId))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(nEntitySync.Event);
                }
            }
        }
    }
}
