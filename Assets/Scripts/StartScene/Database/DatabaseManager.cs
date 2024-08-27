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

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string dbName = "SPACE"; // ��� �̸�
    private string tableName = "USER"; // ���� ���̺� �̸� (uid, email, pw, level)
    private string ip = "43.201.83.32";

    public string rootPW = "";

    private MySqlConnection conn;

    private UserData data; // ���� ���� ����(uid, email, pw, level)


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

                successLogin?.Invoke();
            }
            else
            {
                failLogin?.Invoke();
            }
        }
        catch
        {
            failLogin?.Invoke();
        }
    }

    public void CreateAccount(string email, string pw, Action successLogin, Action failLogin)
    {

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
}