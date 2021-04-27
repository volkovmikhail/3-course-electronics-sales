using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QueryBuilder
{
    static public class QueryBuilderClass
    { 
        public static string QueryBuilder(string WhereCategory, string searchText,bool popularity, bool expensive, bool Pure,CheckedListBox.CheckedItemCollection brandsCollection)
        {
            string command;
            //если выделить все
            if (WhereCategory == "Все категории")
            {
                WhereCategory = "";
            }
            else
            {
                WhereCategory = " category = N'" + WhereCategory + "' AND";
            }
            if (brandsCollection.Count != 0)
            {
                WhereCategory += " brand IN (";
                for (int i = 0; i < brandsCollection.Count; i++)
                {
                    if (brandsCollection.Count-1 != i)
                    {
                        WhereCategory += "'" + brandsCollection[i].ToString() + "',";
                    }
                    else
                    {
                        WhereCategory += "'" + brandsCollection[i].ToString() + "') AND";
                    }
                }
            }
            //создание запроса с параметрами
            if (popularity)
            {
                command = "SELECT name,image,price,id FROM Products WHERE" + WhereCategory + " name LIKE N'%" + searchText + "%' ORDER BY popularity DESC";
            }
            else if (expensive)
            {
                command = "SELECT name,image,price,id FROM Products WHERE" + WhereCategory + " name LIKE N'%" + searchText + "%' ORDER BY price DESC";
            }
            else if (Pure)
            {
                command = "SELECT name,image,price,id FROM Products WHERE" + WhereCategory + " name LIKE N'%" + searchText + "%' ORDER BY price ASC";
            }
            else
            {
                command = "SELECT name,image,price,id FROM Products WHERE category=N'" + WhereCategory + "'";
            }
            return command;
        }
    }
}
