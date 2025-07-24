using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HRMSDAL.Helper;
using HRMSDAL.Service;

namespace HRMSDAL.Service_Implementation
{
    public abstract class GenericService<T> : IGenericService<T> where T : class, new()
    {
        protected abstract string TableName { get; }
        protected abstract string PrimaryKeyColumn { get; }

        protected virtual IEnumerable<string> Columns =>
            TypeDescriptor.GetProperties(typeof(T))
                .Cast<PropertyDescriptor>()
                .Where(p => p.Name != PrimaryKeyColumn)
                .Select(p => p.Name);

        public virtual List<T> GetAll()
        {
            string columnList = string.Join(", ", GetProperties(true).Select(p => p.Name));
            string query = $"SELECT {columnList} FROM {TableName}";

            var result = DBHelper.ExecuteReader(query, CommandType.Text);

            return result.Select(MapDictionaryToEntity).ToList();
        }

        public virtual T GetById(int id)
        {
            string columnList = string.Join(", ", GetProperties(true).Select(p => p.Name));
            string query = $"SELECT {columnList} FROM {TableName} WHERE {PrimaryKeyColumn} = @{PrimaryKeyColumn}";

            var parameters = new IDataParameter[]
            {
            new SqlParameter("@" + PrimaryKeyColumn, id)
            };

            var result = DBHelper.ExecuteReader(query, CommandType.Text, parameters);
            return result.Count > 0 ? MapDictionaryToEntity(result.First()) : null;
        }

        public virtual void Insert(T entity)
        {
            var properties = GetProperties(includePrimaryKey: false);
            string columnList = string.Join(", ", properties.Select(p => p.Name));
            string paramList = string.Join(", ", properties.Select(p => "@" + p.Name));

            string query = $"INSERT INTO {TableName} ({columnList}) VALUES ({paramList})";
            var parameters = MapToParameters(entity, includePrimaryKey: false);

            DBHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public virtual void Update(T entity)
        {
            var properties = GetProperties(includePrimaryKey: false);
            string setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            string query = $"UPDATE {TableName} SET {setClause} WHERE {PrimaryKeyColumn} = @{PrimaryKeyColumn}";
            var parameters = MapToParameters(entity, includePrimaryKey: true);

            DBHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        public virtual void Delete(int id)
        {
            string query = $"DELETE FROM {TableName} WHERE {PrimaryKeyColumn} = @{PrimaryKeyColumn}";

            var parameters = new IDataParameter[]
            {
            new SqlParameter("@" + PrimaryKeyColumn, id)
            };

            DBHelper.ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        protected virtual T MapDictionaryToEntity(Dictionary<string, object> dict)
        {
            T obj = new T();
            foreach (var prop in GetProperties(true))
            {
                object rawValue = dict[prop.Name];
                if (rawValue == null)
                {
                    prop.SetValue(obj, null);
                }
                else
                {
                    Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    object value = Convert.ChangeType(rawValue, targetType);
                    prop.SetValue(obj, value);
                }

            }
            return obj;
        }

        protected virtual IDataParameter[] MapToParameters(T entity, bool includePrimaryKey)
        {
            var props = GetProperties(includePrimaryKey);
            return props.Select(p =>
            {
                object value = p.GetValue(entity) ?? DBNull.Value;
                return new SqlParameter("@" + p.Name, value);
            }).ToArray();
        }

        private IEnumerable<PropertyDescriptor> GetProperties(bool includePrimaryKey)
        {
            return TypeDescriptor.GetProperties(typeof(T))
                .Cast<PropertyDescriptor>()
                .Where(p => includePrimaryKey || p.Name != PrimaryKeyColumn);
        }
    }

}
