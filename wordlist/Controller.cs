using System;
using System.Collections.Generic;
// 
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OleDb;
namespace wordlist
{
    public class Controller
    {
        string constr;
                   
        public  Controller()
        {
            /*constr = @"Data Source=.\SQLEXPRESS;AttachDbFilename=" + 
              Application.StartupPath + @"\wordlist.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";*/

            constr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
              Application.StartupPath + @"\wordlist.accdb";
             //C:\Users\samir\Documents\wordlist.mdf
        }
        public DataSet   ExecuteQuery(string query)
        {
            //SqlDataAdapter sqlAdp = new SqlDataAdapter(query, constr);
            OleDbDataAdapter sqlAdp = new OleDbDataAdapter(query, constr);
            sqlAdp.SelectCommand.CommandText = query;
            DataSet toReturn = new DataSet();
            sqlAdp.Fill(toReturn);
            return toReturn;
        }
        public string[] GetOnlyWords()
        {
            string query = "SELECT word FROM words";
            DataSet ds = this.ExecuteQuery(query);
            string[] toret = new string[ds.Tables[0].Rows.Count];
            int i = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                toret[i] = dr[0].ToString();
                ++i;
            }
            return toret;
        }
        public string[] GetRelatedMeanings(int limit, string where, string fromto)
        {
            string query = "SELECT ";
            if (limit > 0) query += " TOP " + limit.ToString();
            query += " id, word, meaning, pron FROM words " + where;
            if (fromto != "") query += " AND " + fromto + " ";
            query += " ORDER BY word asc";
            DataSet ds = ExecuteQuery(query);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;
            Word[] words = new Word[ds.Tables[0].Rows.Count];
            string[] toret = new string[words.Length];
            if (limit < 0) limit = ds.Tables[0].Rows.Count;

            for (int i = 0; i < ds.Tables[0].Rows.Count && i < limit; ++i)
            {
                Word toReturn = new Word();
                DataRow dr = ds.Tables[0].Rows[i];
                toReturn.id = Convert.ToInt32(dr["id"].ToString());
                toReturn.word = dr["word"].ToString();
                toReturn.meaning = dr["meaning"].ToString();
                //toReturn.sentence = ds["sentences"].ToString();
                toReturn.pron = dr["pron"].ToString();
                revert(toReturn.meaning);
                //revert(toReturn.sentence);
                words[i] = toReturn;
                toret[i] = words[i].meaning;
                //GetWordFromDataRow(ds.Tables[0].Rows[i]);
            }
            //return words;
            return toret;
        }
        public Word[] GetAllWords(int limit, string where, string FROMTO )
        {
            string query = "SELECT " ;
            if(limit > 0) query += " TOP " + limit.ToString();
            query += " id, word, meaning, pron FROM words " + where ;
            if (FROMTO != "") query += " AND " + FROMTO + " ";
            query += " ORDER BY word asc";
            DataSet ds = ExecuteQuery(query);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;
            Word[] words = new Word[ds.Tables[0].Rows.Count];
            
            if (limit < 0) limit = ds.Tables[0].Rows.Count;

            for(int i = 0; i < ds.Tables[0].Rows.Count && i < limit; ++i)
            {
                Word toReturn = new Word();
                DataRow dr = ds.Tables[0].Rows[i];
                toReturn.id = Convert.ToInt32(dr["id"].ToString());
                toReturn.word = dr["word"].ToString();
                toReturn.meaning = dr["meaning"].ToString();
                //toReturn.sentence = ds["sentences"].ToString();
                toReturn.pron = dr["pron"].ToString();
                revert(toReturn.meaning);
                //revert(toReturn.sentence);


                words[i] = toReturn;
                    //GetWordFromDataRow(ds.Tables[0].Rows[i]);
            }
            return words;
        }
        public Word GetWord(string word)
        {
            string query = "SELECT * FROM words WHERE word = '" + word + "'";
            DataSet ds = ExecuteQuery(query);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;
            return GetWordFromDataRow(ds.Tables[0].Rows[0]);
        }
        public void UpdateReport(string word, int cor, int incor)
        { 
            string query = string.Format("UPDATE report SET correct = correct + {0}, incorrect = incorrect + {1} " + 
                            "WHERE word = '{2}'", cor, incor, word);
            DataSet ds = ExecuteQuery(query);
            /*if (ds.Tables.Count == 0)
            {
                query = string.Format("INSERT INTO report values ('{0}', {1}, {2}, {3})", word, cor, incor, 0);
                ExecuteQuery(query);
            }*/
        }
        public Word[] GetAllReportedWords(int limit, string like, string FROMTO, int version)
        {
            string where = "";
            if (version == 1) where = " version = 1 AND ";
            else if (version == 2) where = " version = 2 AND ";


            string query = "SELECT  ";
            if(limit > 0) query += " TOP " + limit.ToString() ;
            query += " words.ID, words.word, words.meaning, words.sentences, words.pron, report.correct, report.incorrect, report.author_id, words.version " + 
                            " FROM            report, words " +
                            " WHERE " + where + " words.word = report.word AND (" + like + ") ";
            if (FROMTO != "") query += " AND " + FROMTO + " ";
            query += " ORDER BY (correct / (correct + incorrect + 1)), incorrect DESC, correct + incorrect ";
            DataSet ds = ExecuteQuery(query);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;
            Word[] w = new Word[ds.Tables[0].Rows.Count];
            int i = 0;
            if (limit < 0) limit = ds.Tables[0].Rows.Count;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (i == limit) break;
                w[i] = new Word();
                w[i] = GetWordFromDataRow(dr);
                w[i].correct = Convert.ToInt32(dr["correct"].ToString());
                w[i].incorrect = Convert.ToInt32(dr["incorrect"].ToString());
                ++i;
            }
            return w;
        }

        public Report GetReport(string word)
        {
            string query = "SELECT * FROM report WHERE word = '" + word + "'";
            DataSet ds = ExecuteQuery(query);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;
            return GetReportFromDataRow(ds.Tables[0].Rows[0]);
        }
        public Report GetReportFromDataRow(DataRow dr)
        {
            Report toReturn = new Report();
            toReturn.word = dr["word"].ToString();
            toReturn.incorrect = Convert.ToInt32(dr["incorrect"].ToString());
            toReturn.correct = Convert.ToInt32(dr["correct"].ToString());
            toReturn.author_id = Convert.ToInt32(dr["author_id"].ToString());
            return toReturn;
        }
        public Word GetWordFromDataRow(DataRow ds)
        {
            Word toReturn = new Word();
            toReturn.id = Convert.ToInt32(ds["id"].ToString());
            toReturn.word = ds["word"].ToString();
            toReturn.meaning = ds["meaning"].ToString();
            toReturn.sentence = ds["sentences"].ToString();
            toReturn.pron = ds["pron"].ToString();
            revert(toReturn.meaning);
            revert(toReturn.sentence);
            
            return toReturn;
        }
        public void AddWord(Word w)
        {
            string query = "SELECT MAX(id) FROM words";
            DataSet ds = ExecuteQuery(query);
            int id = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) + 1;
            w.id = id;
            InsertWord(w);

        }
        public void UpdateWord(Word w, string key)
        {
            w.meaning = w.meaning.Replace("'", "''");
            w.sentence = w.sentence.Replace("'", "''");
            //string query = string.Format("SELECT id FROM words WHERE word = '{0}'", w.word);
            //string id = ExecuteQuery(query).Tables[0].Rows[0][0].ToString();
            string query = string.Format("UPDATE       words " +
                           "SET meaning = '{0}', sentences = '{1}', pron = '{2}', word = '{3}' " +
                            "WHERE word = '{4}'", w.meaning, w.sentence, w.pron, w.word, key);
            ExecuteQuery(query);
            query = "UPDATE report SET word = '" + w.word + "' WHERE word = '" + key + "'";
        }
        private bool ifWordExists(string word)
        {
            DataSet ds = ExecuteQuery(string.Format("SELECT * FROM words WHERE word = '{0}'", word));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;
            return true;
        }
        private bool ifWordReportExists(string word)
        {
            DataSet ds = ExecuteQuery(string.Format("SELECT * FROM report WHERE word = '{0}'", word));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;
            return true;
        }
        public void InsertWord(Word w)
        {
            //adjust(w.sentence);
            //adjust(w.meaning);

            
            if (ifWordExists(w.word)) return;

            w.meaning = w.meaning.Replace("'", "''");
            w.sentence = w.sentence.Replace("'", "''");

            

            
            string query = string.Format("INSERT INTO words VALUES ({0}, '{1}', '{2}', '{3}', '{4}', 0)",
                w.id, w.word, w.meaning, w.sentence, w.pron);
            ExecuteQuery(query);
            
            if (ifWordReportExists(w.word)) return;
            query = string.Format("INSERT INTO report VALUES ('{0}', 0, 0, 0)", w.word);
            ExecuteQuery(query);
        }
        public void revert(string a)
        {
            a = a.Replace("''", "'");
        }
        public void adjust(string a)
        {
            bool h = a.Contains("'");
            a = a.Replace("'", "\'");


            for (int i = 0; i < a.Length; ++i)
            {
                if (a[i] == '\'') a.Insert(i, "\\");
            }
           
        }



        internal Word[] GetLeastAttemptedWords(int limit, string like, string FROMTO, int version)
        {
            string where = "";
            if (version == 1)
            {
                where = " version = 1 AND ";
            }
            else if (version == 2)
            {
                where = " version = 2 AND ";
            }
            string query = "SELECT TOP " + limit.ToString() + " words.ID, words.word, words.meaning, words.sentences, words.pron, report.correct, report.incorrect, report.author_id, correct + incorrect AS attempted, words.version " +
                            "FROM            report, words " +
                            "WHERE " + where + " report.word = words.word AND (" + like + ") ";
            if (FROMTO != "") query += " AND " + FROMTO + " ";
            query +=        "ORDER BY correct + incorrect";
                            
            DataSet ds = ExecuteQuery(query);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return null;
            Word[] w = new Word[ds.Tables[0].Rows.Count];
            int i = 0;
            if (limit < 0) limit = ds.Tables[0].Rows.Count;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (i == limit) break;
                w[i] = new Word();
                w[i] = GetWordFromDataRow(dr);
                w[i].correct = Convert.ToInt32(dr["correct"].ToString());
                w[i].incorrect = Convert.ToInt32(dr["incorrect"].ToString());
                ++i;
            }
            return w;
        }
    }
}
