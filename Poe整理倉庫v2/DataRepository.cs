using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Forms;
using DataGetter;
using LiteDB;

namespace Poe整理倉庫v2
{
    public class DataRepository
    {
        private static readonly string DatabasePath = Path.Combine(Application.StartupPath, "Datas.db");
        private static LiteDatabase LiteDatabase = new LiteDatabase(DatabasePath);

        public static IEnumerable<Data> Find(Expression<Func<Data, bool>> predicate, int skip = 0, int limit = int.MaxValue)
        {
            var c = LiteDatabase.GetCollection<Data>();
            return c.Find(predicate, skip, limit);
        }

        public static Data FindOne(Expression<Func<Data, bool>> predicate)
        {
            var c = LiteDatabase.GetCollection<Data>();
            return c.FindOne(predicate);
        }

        public static IEnumerable<Data> FindAll()
        {
            var c = LiteDatabase.GetCollection<Data>();
            return c.FindAll();
        }

        public static IEnumerable<LiteFileInfo<string>> GetFiles(Expression<Func<LiteFileInfo<string>, bool>> predicate)
        {
            return LiteDatabase.FileStorage.Find(predicate);
        }

        public static LiteFileInfo<string> GetFile(string id)
        {
            return LiteDatabase.FileStorage.FindById(id);
        }
    }
}