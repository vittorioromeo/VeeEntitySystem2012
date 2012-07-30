﻿#region
using System;
using System.Collections.Generic;

#endregion

namespace VeeEntitySystem2012
{
    public class Entity
    {
        private static int _lastUID = -1;

        private readonly Dictionary<Type, Component> _componentDictionary;
        private readonly List<Component> _components;
        private readonly Repository _repository;
        private readonly HashSet<string> _tags;
        private bool _isDead;
        private int _uID;

        public Entity(Manager mManager)
        {
            _lastUID++;
            _uID = _lastUID;

            _components = new List<Component>();
            _componentDictionary = new Dictionary<Type, Component>();
            _tags = new HashSet<string>();

            Manager = mManager;
            _repository = Manager.Repository;
            _repository.AddEntity(this);
        }

        public Manager Manager { get; private set; }

        private void AddTag(string mTag)
        {
            _tags.Add(mTag);
            _repository.TagAdded(this, mTag);
        }
        private void RemoveTag(string mTag)
        {
            _tags.Remove(mTag);
            _repository.TagRemoved(this, mTag);
        }

        private void AddComponent(Component mComponent)
        {
            mComponent.Entity = this;

            var componentType = mComponent.GetType();

            _components.Add(mComponent);

            if (!_componentDictionary.ContainsKey(componentType)) _componentDictionary.Add(componentType, mComponent);
            else _componentDictionary[componentType] = mComponent;

            _repository.ComponentAdded(this, mComponent);

            mComponent.Added();
        }
        internal void RemoveComponent(Component mComponent)
        {
            var componentType = mComponent.GetType();

            _components.Remove(mComponent);
            _componentDictionary.Remove(componentType);
            _repository.ComponentRemoved(this, mComponent);

            mComponent.Removed();
        }

        internal IEnumerable<string> GetTags() { return _tags; }
        internal IEnumerable<Component> GetComponents() { return _components; }

        public void AddTags(params string[] mTags) { foreach (var tag in mTags) AddTag(tag); }
        public void RemoveTags(params string[] mTags) { foreach (var tag in mTags) RemoveTag(tag); }
        public bool HasTag(string mTag) { return _tags.Contains(mTag); }
        public void AddComponents(params Component[] mComponents) { foreach (var component in mComponents) AddComponent(component); }
        public T GetComponent<T>() where T : Component
        {
            if (_componentDictionary.ContainsKey(typeof (T)))
                return (T) _componentDictionary[typeof (T)];

            return null;
        }

        public void Update(float mFrameTime)
        {
            foreach (var component in new List<Component>(_components))
            {
                if (_isDead) return;
                component.Update(mFrameTime);
            }
        }
        public void Destroy()
        {
            _isDead = true;

            foreach (var component in new List<Component>(_components)) RemoveComponent(component);
            _repository.RemoveEntity(this);
        }
    }
}