namespace VeeEntitySystem2012
{
    public abstract class Component
    {
        public Entity Entity { get; internal set; }
        public Manager Manager { get { return Entity.Manager; } }

        public virtual void Added() { }
        public virtual void Removed() { }
        public virtual void Update(float mFrameTime) { }

        public void Destroy() { Entity.RemoveComponent(this); }
    }
}