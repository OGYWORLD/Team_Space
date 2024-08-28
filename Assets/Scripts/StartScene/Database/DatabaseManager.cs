using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySqlConnector;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

/* Database�� �����ϴ� Database Manager �Դϴ�.
 * AWS�� DB ������ �����մϴ�.
 */

public delegate void ChangeUserInfo();

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string dbName = "SPACE"; // ��� �̸�
    private string tableName = "USER"; // ���� ���̺� �̸� (uid, email, pw, level)
    private string ip = "43.203.127.211";

    public string rootPW = "";

    private MySqlConnection conn;

    public UserData data { get; set; } // ���� ���� ����(uid, email, pw, level)

    public ChangeUserInfo changeUserInfo; // ���� ���� ���� �� ȣ��Ǵ� ��������Ʈ

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DBConnect();
    }

    private void DBConnect()
    {
        string config = $"server={ip};port=3306;database={dbName};uid=root;pwd={rootPW};charset=utf8;";

        conn = new MySqlConnection(config);
        conn.Open();
    }

    public void Login(string email, string pw, Action successLogin, Action failLogin)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand();

            pw = EncryptionBySHA256(pw);

            cmd.Connection = conn;
            cmd.CommandText = $"SELECT * FROM {tableName} WHERE email='{email}' AND passwd='{pw}'";

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);

            DataSet set = new DataSet();

            dataAdapter.Fill(set);

            bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

            if (isLoginSuccess)
            {
                DataRow row = set.Tables[0].Rows[0];
                data = new UserData(row);

                changeUserInfo?.Invoke();
                successLogin?.Invoke();
            }
            else
            {
                failLogin?.Invoke();
            }
        }
        catch(Exception e)
        {
            failLogin?.Invoke();
            print($"Error Message {e}");
        }
    }

    public string EncryptionBySHA256(string pw)
    {
        string pwHash = "";

        SHA256 sha256 = SHA256.Create();
        byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(pw));

        foreach (byte b in hashArray)
        {
            pwHash += $"{b:X2}";
        }

        sha256.Dispose();

        return pwHash;
    }

    public bool CheckEmailDuplication(string email)
    {
        MySqlCommand cmd = new MySqlCommand();

        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM {tableName} WHERE email='{email}'";

        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);

        DataSet set = new DataSet();

        dataAdapter.Fill(set);

        return set.Tables[0].Rows.Count == 0;
    }

    public void CreateAccount(string email, string pw, string name, Action SuccessCreate, Action FailCreate)
    {
        try
        {
            pw = EncryptionBySHA256(pw);

            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = $"INSERT INTO {tableName}(email, passwd, name, EXP) VALUES ('{email}','{pw}','{name}',0)";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                SuccessCreate?.Invoke();
            }
            else
            {
                FailCreate?.Invoke();
            }
        }
        catch (Exception e)
        {
            FailCreate?.Invoke();
            print($"Error Message {e}");
        }
    }

    public void EditAccount(string email, string pw, string name, Action SuccessCreate, Action FailCreate)
    {
        try
        {
            pw = EncryptionBySHA256(pw);

            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = $"UPDATE {tableName} SET email='{email}', passwd='{pw}', name='{name}' WHERE uid={data.UID}";

            int queryCount = cmd.ExecuteNonQuery();

            if (queryCount > 0)
            {
                SuccessCreate?.Invoke();
            }
            else
            {
                FailCreate?.Invoke();
            }
        }
        catch (Exception e)
        {
            FailCreate?.Invoke();
            print($"Error Message {e}");
        }
    }

    public void UpdateUserData()
    {
        MySqlCommand cmd = new MySqlCommand();

        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM {tableName} WHERE uid={data.UID}";

        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);

        DataSet set = new DataSet();

        dataAdapter.Fill(set);

        bool isLoginSuccess = set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0;

        if (isLoginSuccess)
        {
            DataRow row = set.Tables[0].Rows[0];

            data.email = row["email"].ToString();
            data.passwd = row["passwd"].ToString();
            data.name = row["name"].ToString();

            changeUserInfo?.Invoke();
        }
    }
}