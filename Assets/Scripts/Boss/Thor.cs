using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thor : Boss
{
    public IEnumerator Pattern1()
    {
        // ���� ��� 1�� ���� �ڷ�ƾ Pattern1() ȣ��

        Vector3 playerPos = new Vector3();
        // TODO �÷��̾��� ���� ��ġ �ľ�
        int historyLevel = BookManager.Instance.checkBookEquipped("Thor1");

        if (true)   // TODO DB���� Thor1�� effect1(�̸� �����ִ� �ð�)�� �������� -> �� ���� ���� ���ǹ� �б�
        {
            // ��ŭ ��ٷ��� �ϴ°� = 1�� - effect1
            yield return new WaitForSeconds(0.1f);
        }

        // HitBoxAreaWarning�� (��ġ �� -> playerPos)�� �� �ܺ� ��ó�� ����
        // effect1 �ð� ��ٸ���
        // ������ HitBoxAreaWarning ��� �����ϱ�
        // ���ϸ�(��ġ) ������ -> �ǰ� ������ ������ �÷��̾� ���
        // ���ϸ��� ���� ���� ������ ���
        // ���ϸ��� ���� ������ �ܺ� ��ó�� �ִ� �÷��̾� ���
        // �ð� ���� ��ٸ��鼭 �ٽ� HitBoxAreaWarning�� (��ġ �� -> �丣)�� ����
        // effect1 �ð� ��ٸ���
        // ������ HitBoxAreaWarning ��� �����ϱ�
        // ���ϸ�(��ġ) ������ -> �ǰ� ������ ������ �÷��̾� ���
        // ���ϸ��� �丣�� ������ ���� ����
    }
}
