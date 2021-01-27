using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Objects.Furniture;
using Revitalize.Framework.Objects.InformationFiles.Furniture;

namespace Revitalize.Framework.Factories.Objects.Furniture
{
    public class TableFactoryInfo:FactoryInfo
    {
        public TableInformation tableInfo;


        public TableFactoryInfo()
        {

        }

        public TableFactoryInfo(TableTileComponent table):base(table)
        {
            this.tableInfo = table.furnitureInfo;
        }

        public TableFactoryInfo(TableMultiTiledObject table):base(table)
        {
            this.tableInfo = null;
        }

    }
}
