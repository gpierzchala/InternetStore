using System;
using System.Collections.Generic;

namespace DataAccess.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Pobranie obiektu z bazy danych
        /// </summary>
        /// <param name="id">id obiektu, który pobieramy z bazy danych</param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Pobranie obiektu z bazy danych
        /// </summary>
        /// <param name="id">Guid obiektu, który pobieramy z bazy danych</param>
        /// <returns></returns>
        T Get(Guid id);

        /// <summary>
        /// Zapisanie obiektu do bazy danych
        /// </summary>
        /// <param name="value">Obiekt, który ma być zapisany w bazie danych</param>
        void Save(T value);

        /// <summary>
        /// Uaktualnienie wartości obiektu w bazie danych
        /// </summary>
        /// <param name="value">Obiekt, który ma być uaktualniony w bazie danych</param>
        void Update(T value);

        /// <summary>
        /// Usunięcie obiektu z bazy danych
        /// </summary>
        /// <param name="value">Obiekt, który ma zostać usunięty z bazy danych</param>
        void Delete(T value);

        /// <summary>
        /// Pobranie listy elementów określonego typu
        /// </summary>
        /// <returns>Lista obiektów określonego typu</returns>
        IList<T> GetAll();
    }
}
