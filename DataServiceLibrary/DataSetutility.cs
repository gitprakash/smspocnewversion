using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using DataModelLibrary;
using System.Collections.Concurrent;
namespace DataServiceLibrary
{
    public static class DataSetutility
    {
        public static  ConcurrentBag<ErrorModal> ValidateStudentTemplate(this DataSet ds)
        {
            ConcurrentBag<ErrorModal> errorlist = new ConcurrentBag<ErrorModal>();
            if (ds != null && ds.Tables.Count > 0)
            {
                var lstcolumns = new List<string> { "RollNo", "Name", "Class", "Section", "Mobile", "Blood Group" };
                bool iscolumnExist = IsAllHeaderColumnExist(ds.Tables[0], lstcolumns);
                if (iscolumnExist)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataSet dsremovedemptyrowSet = RemovedNotFilledRows(ds);
                        if (!IsAllARequiredFieldsFilled(dsremovedemptyrowSet))
                        {
                            errorlist.Add(GetErrorModal("Required field missing","RollNo, Name,Class and Mobile should not be blank"));
                        }
                        else
                        {
                            var tupleunique = IsAllUniqueRollNo(dsremovedemptyrowSet);
                            if (!tupleunique.Item1)
                            {
                                errorlist.AddRange(tupleunique.Item2);
                            }
                            var isValidMobile = IsValidMobile(dsremovedemptyrowSet);
                            if (!isValidMobile.Item1)
                            {
                              errorlist.AddRange(isValidMobile.Item2);
                            }
                        }
                    }
                    else
                    {
                        errorlist.Add(GetErrorModal("Required field missing", "Please enter a student info"));
                    }
                }
                else
                {
                    errorlist.Add(GetErrorModal("Excel File header issue", "Excel Input header format => RollNo, Name,Class, Section ,	Mobile,Blood Group"));
                }
            }
            else
            {
                errorlist.Add(GetErrorModal("Invalid Excel file", "Excel sheet is empty"));
            }
            return errorlist;
        }

        private static ErrorModal GetErrorModal(string errormsg,string errdescr)
        {
            return new ErrorModal
            {
                ErrorMessage = errormsg,
                ErrorDescription = errdescr
            };
        }

        private static Tuple<bool, ConcurrentBag<ErrorModal>> IsAllUniqueRollNo(DataSet ds)
        {
            ConcurrentBag<ErrorModal> lsterror = new ConcurrentBag<ErrorModal>();
            var duplicatcheck =
                ds.Tables[0].AsEnumerable().AsParallel() //.Where(r=>r["Section"]==DBNull.Value)
                    .GroupBy(
                        r =>
                            new
                            {
                                RollNo = r["RollNo"].ToString(),
                                Class = r["Class"].ToString(),
                                Section = r["Section"] == DBNull.Value ? string.Empty : r["Section"]
                            })
                    .Where(dr => dr.Count() > 1);
            if (duplicatcheck.Count() > 0)
            {
                duplicatcheck.ForAll(dupdr =>
                {
                    lsterror.Add(new ErrorModal
                    {
                        ErrorMessage = "Duplicate found",
                        ErrorDescription = string.Format("Duplicate found in  Roll No {0} Class {1} Section {2} ", dupdr.Key.RollNo.ToString(),
                        dupdr.Key.Class.ToString(), dupdr.Key.Section.ToString())
                    });
                });
                return Tuple.Create(false, lsterror);
            }
            else
                return Tuple.Create(true, lsterror);
        }
        private static Tuple<bool, ConcurrentBag<ErrorModal>> IsValidMobile(DataSet ds)
        {
            ConcurrentBag<ErrorModal> errorlist = new ConcurrentBag<ErrorModal>();
            Regex mobileRegex = new Regex(@"^[789]\d{9}$");
            ds.Tables[0].AsEnumerable().AsParallel()
                .Where(dr => mobileRegex.IsMatch(dr["Mobile"].ToString()) == false)
                .ForAll(dr =>
                {
                    errorlist.Add(new ErrorModal
                    {
                        ErrorMessage = string.Format("Invalid Phone - {0}", dr["Mobile"]),
                        ErrorDescription = "Please enter 10 digit moible number"
                    });
                });
            if (errorlist.Count() > 0)
            {
                return Tuple.Create(false, errorlist);
            }
            return Tuple.Create(true, errorlist);
        }

        private static DataSet RemovedNotFilledRows(DataSet ds)
        {
            var notfilledrows = ds.Tables[0].AsEnumerable().Where(r => r["RollNo"] == DBNull.Value &&
                                                                       r["Name"] == null && r["Class"] == DBNull.Value &&
                                                                       r["Mobile"] == DBNull.Value).ToList();

            notfilledrows.ForEach(dr =>
            {
                ds.Tables[0].Rows.Remove(dr);
            });
            ds.Tables[0].AcceptChanges();
            return ds;
        }

        private static bool IsAllARequiredFieldsFilled(DataSet ds)
        {
            var emptyrowcheck = ds.Tables[0].AsEnumerable().SingleOrDefault(r => r["RollNo"] == DBNull.Value ||
                                                                       r["Name"] == DBNull.Value || r["Class"] == DBNull.Value ||
                                                                       r["Mobile"] == DBNull.Value);

            if (emptyrowcheck != null)
            {
                return false;
            }
            return true;
        }

        private static bool IsAllHeaderColumnExist(DataTable tableNameToCheck, List<string> columnsNames)
        {
            bool iscolumnExist = true;
            foreach (string columnName in columnsNames)
            {
                if (!tableNameToCheck.Columns.Contains(columnName))
                {
                    iscolumnExist = false;
                    break;
                }
            }
            return iscolumnExist;
        }

    }
}
