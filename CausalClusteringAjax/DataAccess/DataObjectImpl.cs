using System;
using System.Collections.Generic;
using Library.Data;

namespace Library.Neo4J
{
    public abstract class DataObjectImpl : IDataObject
    {
        protected DataObjectImpl()
        {

        }

        public DateTime TimeCreated { get; internal set; }

        public DateTime TimeLastModified { get; internal set; }

        public Guid Id { get; set; }

        internal void MakeId()
        {
            if (this.Id.Equals(Guid.Empty))
            {
                this.Id = Guid.NewGuid();
                this.TimeCreated = DateTime.UtcNow;
            }
        }

        public virtual bool Update(bool replaceEntry = false)
        {
            using (var tx = NeoStore.BeginTransaction())
            {
                if (this.Id.Equals(Guid.Empty))
                {
                    this.TimeCreated = DateTime.UtcNow;
                }
                this.TimeLastModified = DateTime.UtcNow;
                this.OnUpdating();
                bool ret = NeoStore.Save(this, replaceEntry);
                this.OnUpdated();
                tx.Commit();
                return ret;
            }
        }

        protected virtual void OnUpdating()
        {

        }

        protected virtual void OnUpdated()
        {

        }

        public virtual bool Delete()
        {
            using (var tx = NeoStore.BeginTransaction())
            {
                this.OnDeleting();
                bool ret = NeoStore.Delete(this);
                this.OnDeleted();
                tx.Commit();
                return ret;
            }
        }

        public virtual bool DetachDelete()
        {
            using (var tx = NeoStore.BeginTransaction())
            {
                this.OnDeleting();
                bool ret = NeoStore.DetachDelete(this);
                this.OnDeleted();
                tx.Commit();
                return ret;
            }
        }

        protected virtual void OnDeleting()
        {

        }

        protected virtual void OnDeleted()
        {

        }

        public virtual void AcquireWriteLock()
        {
            NeoStore.AcquireWriteLock(this);
        }

        public virtual HashSet<string> Labels()
        {
            return new HashSet<string>();
        }

        public override bool Equals(object obj)
        {
            DataObjectImpl record = obj as DataObjectImpl;

            return !Object.ReferenceEquals(null, record)
                && this.Id.Equals(record.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
