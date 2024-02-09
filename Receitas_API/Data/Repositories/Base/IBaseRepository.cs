using System.Collections.Generic;
using System.Data;

namespace Receitas_API.Data.Repositories.Base
{
    public interface IBaseRepository<T>
    {
        long Insert(T entity);
        void Update(T entity);
        void DeleteById(int Id);

        bool EntradaExiste_BD(string campo, string str);

        int GetFirstId();
        int GetLastId();
        IEnumerable<T> Query(string where = null);
        T Query_ById(int Id);
        int GetIdByDescription(string Description);
        string GetDescriptionById(int Id);

        bool RecInUse(int Id);
        bool RecInUse(string sDescricao);
        bool TableHasData();

        List<T> Listar();
        IEnumerable<T> ListByDescription(string Description);
        IDataReader ListByDescription_Reader(string Descricao);
        IEnumerable<T> Listar_ID(int Id);
        IDataReader Listar_ID_DR(int Id);
    }
}