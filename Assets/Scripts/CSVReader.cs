using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// https://github.com/salt26/chordingcoding/blob/master/ChordingCoding/Util.cs
public class CSVReader
{
    StreamReader streamReader;
    List<List<string>> data = new List<List<string>>();
    List<string> header = new List<string>();
    bool hasHeader = false;
    Dictionary<int, List<string>> columns = new Dictionary<int, List<string>>();

    /// <summary>
    /// filename�� .csv ������ �а� �� �����͸� �����Ͽ� �����մϴ�.
    /// </summary>
    /// <param name="filename">���� �̸�(���)</param>
    /// <param name="hasHeader">ù �ٿ� ����� ���� true, ��� ���� �����Ͱ� �ٷ� ���� false</param>
    /// <param name="delimiter">�� ���� ����</param>
    public CSVReader(string filename, bool hasHeader, char delimiter = ',')
    {
        streamReader = new StreamReader(filename, Encoding.GetEncoding("UTF-8"));
        int i = 0;
        if (hasHeader)
        {
            i = -1;
            this.hasHeader = true;
        }
        while (!streamReader.EndOfStream)
        {
            string s = streamReader.ReadLine();
            if (i >= 0)
            {
                data.Add(new List<string>());
                string[] temp = s.Split(delimiter);
                for (int j = 0; j < temp.Length; j++)
                {
                    data[i].Add(temp[j]);
                }
            }
            else
            {
                string[] temp = s.Split(delimiter);
                for (int j = 0; j < temp.Length; j++)
                {
                    header.Add(temp[j]);
                }
            }
            i++;
        }
    }

    /// <summary>
    /// �־��� headerName�� �� ��° ��(0���� ����)�� ��� �̸����� ��ȯ�մϴ�.
    /// ������ -1�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="headerName">��� �̸�</param>
    /// <returns></returns>
    public int GetHeaderIndex(string headerName)
    {
        return header.IndexOf(headerName);
    }

    /// <summary>
    /// index(0���� ����)��° ���� �����͸� ��ȯ�մϴ�.
    /// ��ȯ�� �����͸� �Ժη� �������� ������.
    /// </summary>
    /// <param name="index">�� �ε��� (0 �̻�)</param>
    /// <returns></returns>
    public List<string> GetRow(int index)
    {
        if (index < 0 || index >= data.Count) return null;
        return data[index];
    }

    /// <summary>
    /// headerIndex(0���� ����)��° ���� �����͸� ��ȯ�մϴ�.
    /// Ư�� headerIndex�� ���� ó�� ȣ���ϸ� �ð��� ���� �ɸ� �� �ֽ��ϴ�.
    /// �������� � �࿡ headerIndex��° ���� ������
    /// �� ���� ���� null�� ä�����ϴ�.
    /// </summary>
    /// <param name="headerIndex">�� �ε��� (0 �̻�)</param>
    /// <returns></returns>
    public List<string> GetColumn(int headerIndex)
    {
        if (headerIndex < 0)
        {
            return null;
        }
        else if (columns.ContainsKey(headerIndex))
        {
            return columns[headerIndex];
        }
        else
        {
            List<string> column = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                if (headerIndex < data[i].Count)
                {
                    column.Add(data[i][headerIndex]);
                }
                else
                {
                    column.Add(null);
                }
            }
            columns.Add(headerIndex, column);
            return column;
        }
    }

    /// <summary>
    /// headerName�� ��� �̸����� �ϴ� ���� �����͸� ��ȯ�մϴ�.
    /// Ư�� headerName�� ���� ó�� ȣ���ϸ� �ð��� ���� �ɸ� �� �ֽ��ϴ�.
    /// </summary>
    /// <param name="headerName">��� �̸�</param>
    /// <returns></returns>
    public List<string> GetColumn(string headerName)
    {
        return GetColumn(GetHeaderIndex(headerName));
    }

    /// <summary>
    /// ����� �ִ� ��� ��� ����� ��ȯ�մϴ�.
    /// ������ null�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public List<string> GetHeader()
    {
        if (hasHeader)
            return header;
        else return null;
    }

    /// <summary>
    /// ������ ��ü�� ��ȯ�մϴ�.
    /// ��ȯ�� ���� �迭�� ù ��° �ε����� �� �ε���,
    /// �� ��° �ε����� �� �ε����Դϴ�.
    /// ��ȯ�� �����͸� �Ժη� �������� ������.
    /// </summary>
    /// <returns></returns>
    public List<List<string>> GetData()
    {
        return data;
    }
}
