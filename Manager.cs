#region
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace VeeEntitySystem2012
{
    public class Manager
    {
        internal readonly Repository Repository;

        public Manager() { Repository = new Repository(); }

        public void Clear() { Repository.Clear(); }

        public IEnumerable<Entity> GetEntities() { return Repository.GetEntities(); }
        public IEnumerable<Entity> GetEntitiesByTag(string mTag) { return Repository.GetEntitiesByTag(mTag); }
        public IEnumerable<Entity> GetEntitiesByComponent(Type mType) { return Repository.GetEntitiesByComponent(mType); }
        public IEnumerable<Component> GetComponents(Type mType) { return Repository.GetComponents(mType); }

        public bool HasEntityByTag(string mTag) { return GetEntitiesByTag(mTag).Any(); }
        public bool HasEntityByComponent(Type mType) { return GetEntitiesByComponent(mType).Any(); }

        public void Update(float mFrameTime)
        {
            foreach (var entity in new List<Entity>(Repository.GetEntities()))
                entity.Update(mFrameTime);
        }
    }
}