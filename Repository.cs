#region
using System;
using System.Collections.Generic;

#endregion

namespace VeeEntitySystem2012
{
    internal class Repository
    {
        private readonly Dictionary<Type, HashSet<Entity>> _componentEntities;
        private readonly Dictionary<Type, HashSet<Component>> _components;
        private readonly HashSet<Entity> _entities;
        private readonly Dictionary<string, HashSet<Entity>> _taggedEntities;

        internal Repository()
        {
            _entities = new HashSet<Entity>();
            _taggedEntities = new Dictionary<string, HashSet<Entity>>();
            _componentEntities = new Dictionary<Type, HashSet<Entity>>();
            _components = new Dictionary<Type, HashSet<Component>>();
        }

        internal void Clear()
        {
            _entities.Clear();
            _taggedEntities.Clear();
            _componentEntities.Clear();
            _components.Clear();
        }

        internal void AddEntity(Entity mEntity)
        {
            _entities.Add(mEntity);

            foreach (var tag in mEntity.GetTags())
            {
                if (!_taggedEntities.ContainsKey(tag)) _taggedEntities.Add(tag, new HashSet<Entity>());
                _taggedEntities[tag].Add(mEntity);
            }

            foreach (var component in mEntity.GetComponents())
            {
                var componentType = component.GetType();

                if (!_componentEntities.ContainsKey(componentType))
                {
                    _componentEntities.Add(componentType, new HashSet<Entity>());
                    _components.Add(componentType, new HashSet<Component>());
                }

                _componentEntities[componentType].Add(mEntity);
                _components[componentType].Add(component);
            }
        }
        internal void RemoveEntity(Entity mEntity)
        {
            _entities.Remove(mEntity);

            foreach (var tag in mEntity.GetTags()) _taggedEntities[tag].Remove(mEntity);
            foreach (var component in mEntity.GetComponents())
            {
                var componentType = component.GetType();
                _componentEntities[componentType].Remove(mEntity);
                _components[componentType].Remove(component);
            }
        }

        internal void TagAdded(Entity mEntity, string mTag)
        {
            if (!_taggedEntities.ContainsKey(mTag)) _taggedEntities.Add(mTag, new HashSet<Entity>());
            _taggedEntities[mTag].Add(mEntity);
        }
        internal void TagRemoved(Entity mEntity, string mTag) { if (_taggedEntities.ContainsKey(mTag)) _taggedEntities[mTag].Remove(mEntity); }

        internal void ComponentAdded(Entity mEntity, Component mComponent)
        {
            var componentType = mComponent.GetType();

            if (!_components.ContainsKey(componentType))
            {
                _components.Add(componentType, new HashSet<Component>());
                _componentEntities.Add(componentType, new HashSet<Entity>());
            }

            _components[componentType].Add(mComponent);
            _componentEntities[componentType].Add(mEntity);
        }
        internal void ComponentRemoved(Entity mEntity, Component mComponent)
        {
            var componentType = mComponent.GetType();

            _components[componentType].Remove(mComponent);
            _componentEntities[componentType].Remove(mEntity);
        }

        internal IEnumerable<Entity> GetEntities() { return _entities; }
        internal IEnumerable<Entity> GetEntities(string mTag)
        {
            if (_taggedEntities.ContainsKey(mTag)) return _taggedEntities[mTag];
            return new Entity[] {};
        }
        internal IEnumerable<Entity> GetEntitiesByComponent(Type mType)
        {
            if (_componentEntities.ContainsKey(mType)) return _componentEntities[mType];
            return new Entity[] {};
        }
        internal IEnumerable<Component> GetComponents(Type mType)
        {
            if (_components.ContainsKey(mType)) return _components[mType];
            return new Component[] {};
        }
    }
}