using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace MASSemanticWeb
{
    /// <summary>
    /// Статический класс для импорта-экспорта СС
    /// </summary>
    static class ImportExport
    {
        //0. открыть owl
        /// <summary>
        /// Открытие owl файла с СС
        /// </summary>
        /// <param name="pathToOWL">Путь до owl файла</param>
        /// <returns>СС во внутреннем представлении. null в случае неудачи.</returns>
        static public SemanticWeb OpenOWLFile(string pathToOWL)
        {
            SemanticWeb _web=new SemanticWeb();
            //0. Открываем файл как XMl
            XmlTextReader _reader = new XmlTextReader(pathToOWL);
            try
            {
                //1. Разбираем
                while (_reader.Read())
                    if (_reader.IsStartElement())
                    {
                        Console.WriteLine(_reader.Name.ToUpper());
                        switch (_reader.Name.ToUpper())
                        {
                            case "DECLARATION":
                                ParseDeclaration(_reader, _web);
                                break;
                            case "SUBCLASSOF":
                                ParseProperty(_reader, _web);
                                break;
                            case "ENTITYANNOTATION":
                                ParseAnnotation(_reader, _web);
                                break;
                            default:
                                _reader.Read();
                                break;
                        }
                    }
                _reader.Close();
            }
            catch
            {
                _reader.Close();
                return null;
            }
            //2. PROFIT
            return _web;
        }

        /// <summary>
        /// Разбор блока описания. Мб быть класс/связь
        /// </summary>
        /// <param name="reader">Откуда читаем</param>
        /// <param name="web">СС</web>
        private static void ParseDeclaration(XmlTextReader reader, SemanticWeb web)
        {
            //Пропускаем пустое пространство
            while (reader.Name.ToUpper() != "CLASS" && reader.Name.ToUpper() != "OBJECTPROPERTY")
                reader.Read();
            switch (reader.Name.ToUpper())
            {
                case "CLASS":
                    ParseClassDeclaration(reader,web);
                    reader.Read();
                    break;
                case "OBJECTPROPERTY":
                    ParsePropertyDeclaration(reader, web);
                    reader.Read();
                    break;
            }
        }

        /// <summary>
        /// Разбор описания связи
        /// </summary>
        /// <param name="reader">Откуда читаем</param>
        /// <param name="web">СС</param>
        private static void ParsePropertyDeclaration(XmlTextReader reader, SemanticWeb web)
        {
            if (!web.Arcs.Exists(arc=>arc.Name==reader.GetAttribute("URI").Split(';')[1]))//(web.Arcs.Find(arc => arc.Name.ToUpper() == reader.GetAttribute("URI").Split(';')[1])==null)
                //такой связи еще нет. Создаем
                web.AddArc(reader.GetAttribute("URI").Split(';')[1],"", Color.Aqua, null);  
        }

        /// <summary>
        /// Разбор описания класса (узла)
        /// </summary>
        /// <param name="reader">Откуда читаем</param>
        /// <param name="web">СС</param>
        private static void ParseClassDeclaration(XmlTextReader reader, SemanticWeb web)
        {
            //TODO: Задавать координаты при создании?
            if (!web.Nodes.Exists(node => node.Name == reader.GetAttribute("URI").Split(';')[1])) 
                web.AddNode(reader.GetAttribute("URI").Split(';')[1],"",new Point(0,0));  
        }

        /// <summary>
        /// Разбор связи между вершинами
        /// </summary>
        /// <param name="reader">Откуда читаем</param>
        /// <param name="web">СС</web>
        private static void ParseProperty(XmlTextReader reader, SemanticWeb web)
        {
            while (reader.Name.ToUpper() != "CLASS")
                reader.Read();
            SemanticNode _firstNode = web.Nodes.Find(node => node.Name == reader.GetAttribute("URI").Split(';')[1]);
            if (_firstNode == null)
            {
                web.AddNode(reader.GetAttribute("URI").Split(';')[1], "", new Point(0, 0));
                _firstNode = web.Nodes.Find(node => node.Name == reader.GetAttribute("URI").Split(';')[1]);
            }
            reader.Read();
            SemanticArc _arc;
            SemanticNode _secondNode;
            while (reader.Name == "")
                reader.Read();
            //возможно два варианта, либо это is-a, либо другая связь
            //Это другая связь
            if (reader.Name == "ObjectSomeValuesFrom")
            {
                reader.Read();
                while (reader.Name == "")
                    reader.Read();
                _arc = web.Arcs.Find(arc => arc.Name == reader.GetAttribute("URI").Split(';')[1]);
                if (_arc == null)
                {
                    web.AddArc(web.Arcs.Count, reader.GetAttribute("URI").Split(';')[1], "", Color.Aqua, null);
                    _arc = web.Arcs.Find(arc => arc.Name == reader.GetAttribute("URI").Split(';')[1]);
                }
                reader.Read();
                while (reader.Name == "")
                    reader.Read();
                _secondNode = web.Nodes.Find(node => node.Name == reader.GetAttribute("URI").Split(';')[1]);
                if (_secondNode == null)
                {
                    web.AddNode(reader.GetAttribute("URI").Split(';')[1], "", new Point(0, 0));
                    _secondNode = web.Nodes.Find(node => node.Name == reader.GetAttribute("URI").Split(';')[1]);
                }
                web.AddArcBetweenNodes(_firstNode, _secondNode, _arc, false);
            }
            //Это is-a
            else
            {
                _secondNode = web.Nodes.Find(node => node.Name == reader.GetAttribute("URI").Split(';')[1]);
                if (_secondNode == null)
                {
                    web.AddNode(reader.GetAttribute("URI").Split(';')[1], "", new Point(0, 0));
                    _secondNode = web.Nodes.Find(node => node.Name == reader.GetAttribute("URI").Split(';')[1]);
                }
                _arc = web.Arcs.Find(arc => arc.Id == 0);
                web.AddArcBetweenNodes(_secondNode, _firstNode, _arc, false);
            }
            reader.Read();
        }

        /// <summary>
        /// Разбор аннотации
        /// </summary>
        /// <param name="reader">Откуда читаем</param>
        /// <param name="web">СС</web>
        private static void ParseAnnotation(XmlTextReader reader, SemanticWeb web)
        {
             while (reader.Name.ToUpper() != "CLASS")
                reader.Read();
            String _nodeName=reader.GetAttribute("URI").Split(';')[1];
            reader.Read();
            while (reader.Name.ToUpper() != "ANNOTATION")
                reader.Read();
            if (reader.GetAttribute("annotationURI") == "&rdfs;comment")
            {
                reader.Read();
                while (reader.Name.ToUpper() == "")
                    reader.Read();
                web.Nodes.Find(node => node.Name == _nodeName).Comment = reader.ReadElementContentAsString();
            }
        }

        //1. сохранить owl
        //2. открыть xml
        //3. сохранить xml

    }
}
