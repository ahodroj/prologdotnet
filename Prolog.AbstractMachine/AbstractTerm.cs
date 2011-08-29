using System;
using System.Collections;
using System.Text;
using System.Reflection;

namespace Axiom.Runtime
{
    /// <summary>
    /// Provides a container for an abstract term.
    /// </summary>
    public class AbstractTerm : HeapNode
    {
        private static int _variableIndex = 0;

        private string _variableName = "";
        /// <summary>
        /// Provides a container for another abstract term.
        /// </summary>
        private AbstractTerm _containee = null;

        /// <summary>
        /// A reference to another abstract term.
        /// </summary>
        private AbstractTerm _reference;

        /// <summary>
        /// default constructor.
        /// </summary>
        public AbstractTerm()
        {
            _variableName += "_" + _variableIndex;
            _reference = this;
            _variableIndex++;
        }

        /// <summary>
        /// Binds this object to another one.
        /// </summary>
        /// <param name="term"></param>
        public virtual void Bind(AbstractTerm term)
        {
            AbstractTerm t1 = this;
            AbstractTerm t2 = term;

            AMTrail trail = AMTrail.Instance;

            // if t1 is a reference
            //   and 
            //  (t2 is not a reference) or address2 < address1
            if (t1.IsReference && !t2.IsReference)
            {
                Assign(t2);
                trail.Trail(t1);
            }
            else
            {
                t2.Assign(t1);
                trail.Trail(t2);
            }
        }

        /// <summary>
        /// Returns object data.
        /// </summary>
        /// <returns></returns>
        public virtual object Data()
        {
            if (IsAssigned())
            {
                return _containee.Data();
            }
            return null;
        }

        /// <summary>
        /// Dereferences this object to the object it is assigned to.
        /// </summary>
        /// <returns></returns>
        public virtual AbstractTerm Dereference()
        {
            // in case we are wrapping a variable
            
            if (IsAssigned())
            {
                return _containee.Dereference();
            }

            return this;
             
            //if (_containee == null || _containee == this)
            //{
            //    return this;
            //}
            //else
            //{
            //    AbstractTerm reference = _containee;
            //    while (reference._containee != null)
            //    {
            //        _reference = _reference._containee;
            //    }
            //    return _reference;
            //}
            //return null;
        }

        /// <summary>
        /// Returns the reference of this object.
        /// </summary>
        /// <returns></returns>
        public virtual AbstractTerm Reference()
        {
            if (IsAssigned())
            {
                return _containee.Reference();
            }
            return this;
        }

        /// <summary>
        /// Assigns this object to another one.
        /// </summary>
        /// <param name="term"></param>
        public void Assign(AbstractTerm term)
        {
            //if (term._containee == this)
            //{
            //    return;
            //}
            //_containee = term;
            _containee = term.Dereference();
            
        }

        public virtual bool Unify(AbstractTerm term)
        {
            if (IsAssigned())
            {
                return _containee.Unify(term);
            }
            
            // perform unification here
            if (this == term)
            {
                return true;
            }

            if (!term.IsReference)
            {
                this.Assign(term);
                return true;
            }
            else
            {
                this.Bind(term);
                return true;
            }
        }

        public void Unbind()
        {
            _containee = null;
            _reference = this;
        }

        public virtual AbstractTerm this[int index]
        {
            get 
            {
                if (IsAssigned())
                {
                    return _containee[index];
                }

                if (index == 0)
                {
                    return (AbstractTerm)_next;
                }
                else
                {
                    AbstractTerm vPtr = (AbstractTerm)_next;
                    for (int j = 0; j < index; j++)
                    {
                        vPtr = (AbstractTerm)vPtr.Next;
                    }
                    return vPtr;
                }
            }

            
        }

        public virtual AbstractTerm Head
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.Head;
                }
                return null;
            }
        }

        public virtual AbstractTerm Tail
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.Tail;
                }
                return null;
            }
        }


        public virtual bool IsReference
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.IsReference;
                }
                return true;
            }
        }

        public virtual bool IsConstant
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.IsConstant;
                }
                return false;
            }
        }

        public virtual bool IsStructure
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.IsStructure;
                }
                return false;
            }
        }

        public virtual bool IsList
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.IsList;
                }
                return false;
            }
        }

        public virtual bool IsObject
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.IsObject;
                }
                return false;
            }
        }

        public virtual int Arity
        {
            get 
            {
                if (IsAssigned())
                {
                    return _containee.Arity;
                }
                return 0;
            }

            set
            {
                if (IsAssigned())
                {
                    _containee.Arity = value;
                }
            }
        }

        public virtual string Name
        {
            get
            {
                if (IsAssigned())
                {
                    return _containee.Name;
                }
                return _variableName;
            }

            set
            {
                if (IsAssigned())
                {
                    _containee.Name = value;
                }
                else
                {
                    _variableName = value;
                }
            }
        }

        public override string ToString()
        {
            if (IsAssigned())
            {
                return _containee.ToString();
            }
            return Name;
        }

        private bool IsAssigned()
        {
            return _containee != null && _containee != this;
        }

        public void Copy(AbstractTerm term)
        {
            this._containee = term._containee;
            this._reference = term._reference;
            
        }
    }
}
