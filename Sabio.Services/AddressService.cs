using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Sabio.Models.Requests;

namespace Sabio.Services
{
    public class AddressService
    {
        IDataProvider _data = null;
        public AddressService(IDataProvider data)
        {
            _data = data;
        }

        public void Update(AddressUpdateRequest model)
        {          
            string procName = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@LineOne", model.LineOne);
                    col.AddWithValue("@SuiteNumber", model.SuiteNumber);
                    col.AddWithValue("@City", model.City);
                    col.AddWithValue("@State", model.State);
                    col.AddWithValue("@PostalCode", model.PostalCode);
                    col.AddWithValue("@IsActive", model.IsActive);
                    col.AddWithValue("@Lat", model.Lat);
                    col.AddWithValue("@Long", model.Long);
                    col.AddWithValue("@Id", model.Id);

                }, returnParameters: null);
        }

        public int Add(AddressAddRequest model)
        {
            int id = 0;
            string procName = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@LineOne", model.LineOne);
                    col.AddWithValue("@SuiteNumber", model.SuiteNumber);
                    col.AddWithValue("@City", model.City);
                    col.AddWithValue("@State", model.State);
                    col.AddWithValue("@PostalCode", model.PostalCode);
                    col.AddWithValue("@IsActive", model.IsActive);
                    col.AddWithValue("@Lat", model.Lat);
                    col.AddWithValue("@Long", model.Long);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);

                }, returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object objId = returnCollection["@Id"].Value;

                    Int32.TryParse(objId.ToString(), out id);

                    Console.WriteLine("");

                });
            return id;
        }

        public Addresses Get(int id)
        {
            
            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Addresses address = null; 

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection) {
                
                parameterCollection.AddWithValue("@Id", id);

            }, delegate(IDataReader reader, short set)
            {               
                address = MapAddress(reader);
            }
            );
            return address;
        }

        public List<Addresses> GetTop()
        {
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            List<Addresses> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Addresses address = MapAddress(reader);

                if (list == null)
                {
                    list = new List<Addresses>();
                }

                list.Add(address);

            });

            return list;
        }

        private static Addresses MapAddress(IDataReader reader)
        {
            Addresses address = new Addresses();

            address = new Addresses();

            int i = 0;

            address.Id = reader.GetSafeInt32(i++);
            address.LineOne = reader.GetSafeString(i++);
            address.SuiteNumber = reader.GetSafeInt32(i++);
            address.City = reader.GetSafeString(i++);
            address.State = reader.GetSafeString(i++);
            address.PostalCode = reader.GetSafeString(i++);
            address.IsActive = reader.GetSafeBool(i++);
            address.Lat = reader.GetSafeDouble(i++);
            address.Long = reader.GetSafeDouble(i++);
            return address;
        }
    }
}
