[System.Serializable]
public class CMD_Head
{
    public int head = Constant.SOCKET_PACKAGE_HEAD;        //包头标志
    public int length;      //从 magic 开始的经过压缩、加密的总长度
    public int magic;       //用于解密校验
    public int api;         //api接口标志
    public short version = Constant.SOCKET_PACKAGE_VERSION;   //api版本
    public int identifier = Constant.SOCKET_PACKAGE_IDENTIFIER_DEFAULT;  //客户端请求标志位，用于前端标记请求
    public CMD_Body body = new CMD_Body();  //数据列表
}

[System.Serializable]
public class CMD_Body
{
    public short type = Constant.SOCKET_PACKAGE_BODY_TYPE_JSON;      //数据类型  1   json, 2 binary 二进制数据, 3 protostuff
    public short compress = Constant.SOCKET_PACKAGE_BODY_UNCOMPRESSED;  //数据压缩方式 0不压缩
    public int dataSize;    //数据长度
    public string data;     //数据
    // public byte[] data2;    //数据
}

[System.Serializable]
public class HeartBeat
{
    public long ts;
}

[System.Serializable]
public class RequestResult<T>
{
    public T result;
    public int ret;
    public string msg;

    public bool Success()
    {
        return ret == 0;
    }
}