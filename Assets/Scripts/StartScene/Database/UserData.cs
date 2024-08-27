
using System.Data;

public class UserData
{
    private int uid;
    public int UID => uid;

    public string email;
    public string passwd { get; set; }

    public int lv;

    public UserData(string email, string passwd, int lv)
    {
        this.email = email;
        this.passwd = passwd;
        this.lv = lv;
    }

    public UserData(DataRow row)
    {
        this.uid = int.Parse(row["uid"].ToString());
        this.email = row["email"].ToString();
        this.passwd = row["passwd"].ToString();
        this.lv = int.Parse(row["lv"].ToString());
    }
}
