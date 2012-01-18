using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using ConfArt.Core;
using System.Xml.Linq;
using ArtOfNet.ConfArt.Exceptions;
using System.Reflection;

namespace ArtOfNet.ConfArt.Core
{
    public class DynamicXmlElement : DynamicConvertibleBase
    {
        private XElement _InternalNode;
        private readonly object _syncRoot = new object();

        public DynamicXmlElement(XElement node)
        {
            this._InternalNode = node;
        }

        public DynamicXmlElement()
        {
        }

        public DynamicXmlElement(string source)
        {
            try 
	        {
                if (source.Contains("<"))
                {
                    this._InternalNode = XElement.Parse(source);
                }
                else
                {
                    this._InternalNode = new XElement(source);
                }
	        }
	        catch (Exception inner)
	        {
                DynamicXmlParseException ex = new DynamicXmlParseException("Error while loading source xml in a DynamicXmlElement constructor", inner);
                ex.Data.Add("DynamicXmlElement.Contructor.source", source);
		        throw ex;
	        }
        }

        private bool TrySetXmlMember(string name, object value)
        {
            XElement setNode = _InternalNode.Element(name);
            if (setNode != null)
                setNode.SetValue(value);
            else
            {
                if (value.GetType() == typeof(DynamicXmlElement))
                {
                    _InternalNode.Add(new XElement(name));
                }
                else
                {
                    if (value.GetType() == typeof(XElement) || value.GetType() == typeof(XAttribute))
                    {
                        _InternalNode.Add(value);
                    }
                    else
                    {
                        _InternalNode.Add(new XElement(name, value));
                    }
                }

            }
            return true;
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            return TrySetXmlMember(binder.Name, value);
        }

        private bool TryGetXmlMember(string name, out object result)
        {
            //child name should map to one or many elements, so let's use a list by default
            IEnumerable<XElement> nodes = _InternalNode.Elements(name);


            //if something found it may be a _InternalNode or a list of child nodes
            if (nodes != null && nodes.Count() > 0)
            {
                if (nodes.Count() == 1)
                {
                    XElement current = nodes.First();
                    if (current.HasElements)
                    {
                        List<string> list = new List<string>(nodes.Elements().Select(x => x.Value));
                        result = list;
                    }
                    else
                    {
                        result = new DynamicXmlElement(nodes.First());
                    }
                    
                }
                else
                {
                    List<DynamicXmlElement> list = new List<DynamicXmlElement>(nodes.Select(x => new DynamicXmlElement(x)));
                    result = list;
                }
                return true;
            }
            //else it should be an attribute name
            else
            {

                IEnumerable<XAttribute> getAttrib = _InternalNode.Attributes(name);
                //finally try to see if the call is made to a member of XElement
                if (getAttrib != null && getAttrib.Count() > 0)
                {
                    //if it matches at least one attribute, return directly an XAttribue, as you don't want to
                    //map it as a dynamic node (after all, we can view it as a terminal leaf in a dynamic tree)
                    if (getAttrib.Count() > 1)
                    {
                        result = getAttrib;
                    }
                    else
                    {
                        result = getAttrib.First().Value;
                    }
                    return true;
                }
                else
                {
                    Type xet = typeof(XElement);
                    MemberInfo[] members = xet.GetMember(name);
                    if (members != null && members.Count() > 0)
                    {
                        MemberInfo one = members[0];
                        if (one is PropertyInfo)
                        {
                            var property = (PropertyInfo)one;
                            result = property.GetValue(_InternalNode, null);
                            return true;
                        }
                        else if (one is MethodInfo)
                        {
                            var method = (MethodInfo)one;
                            result = method.Invoke(_InternalNode, null);
                            return true;
                        }
                        result = new DynamicXmlElement(new XElement(name));
                        return true;
                    }
                    else
                    {
                        result = new DynamicXmlElement(new XElement(name));
                        return true;
                    }
                }
            }
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            return TryGetXmlMember(binder.Name, out result);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length == 0)
            {
                result = null;
                return false;
            }

            string key = indexes[0] as string;
            if (string.IsNullOrEmpty(key))
            {
                result = null;
                return false;
            }

            return TryGetXmlMember(key, out result);
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes.Length == 0)
            {
                return false;
            }
            string key = indexes[0] as string;
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            lock (_syncRoot)
            {
                XElement child = _InternalNode.Element(key);

                if (child != null) 
                {
                    child = value as XElement;
                }
                else
                {
                    _InternalNode.Add(value);
                }
            }
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Keys;
        }

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (string childNode in _InternalNode.Elements().Select(x => x.Name.LocalName))
                {
                    yield return childNode;
                }
                foreach (string childNode in _InternalNode.Attributes().Select(x => x.Name.LocalName))
                {
                    yield return childNode;
                }
            }
        }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (XElement elem in _InternalNode.Elements())
            {
                result.Add(
                    elem.Name.ToString(), (object)elem.Value);
            }
            foreach (XAttribute elem in _InternalNode.Attributes())
            {
                result.Add(
                    elem.Name.ToString(), (elem.Value));
            }
            return result;
        }

        public object ToObject()
        {
            return (object)ToDictionary();

        }

        public override string ToString()
        {
            return _InternalNode.Value;
        }

        

    }
}
