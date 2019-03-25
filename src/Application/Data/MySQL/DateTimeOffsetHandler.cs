using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ContosoTravel.Web.Application.Data.MySQL
{
    public class DateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset?>
    {
        public override void SetValue(IDbDataParameter parameter, DateTimeOffset? value)
        {
            parameter.Value = (object)value?.UtcDateTime ?? System.DBNull.Value;
        }

        public override DateTimeOffset? Parse(object value)
        {
            if ( value == null || value == System.DBNull.Value )
            {
                return null;
            }

            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
        }
    }
}
