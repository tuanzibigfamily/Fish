
////修改CFish类，实现如下功能：
////(1) 增加一个角度m_Angle表示游动的角度，m_Angle角度的值可为正值或负值，其中逆时针方向为正值，顺时针方向为负值；
////(2) 如果m_Angle角度为正值并且小于65度时，在每次更新时，随机地进入逆时针游动状态或顺时针游动状态； 
////(3) 如果m_Angle角度为负值并且大于-65度时，在每次更新时，随机地进入逆时针游动状态或顺时针游动状态；
////(4) 在逆时针游动状态，随机持续5-10次更新时对m_Angle角度加上2.0度；
////(5) 在顺时针游动状态，随机持续5-10次更新时对m_Angle角度减小-2.0度；
////(6) 如果m_Angle角度为正值并且大于65度时，在每次更新时，进入顺时针游动状态； 
////(7) 如果m_Angle角度为负值并且小于-65度时，在每次更新时，进入在逆时针游动状态； 
////(8) 修改Move方法，按照角度m_Angle来向前游动2.0的距离，实现位置的m_X坐标，m_Y坐标的更新；
using System;
using System.Drawing;
using GameLib;

namespace MyFishGame
{
	public class CFish
	{
		double m_X;
		double m_Y;

		GameImage m_TexImage;

		int m_ImageIndex;

		int m_ImageCountPreRow;
		int m_ImageMaxIndex;

		int m_UpateLimit;
		long m_LastTickCount;

		double m_Angle;  // 新增角度，单位是度，表示鱼的游动角度
		int m_DirectionChangeCounter;  // 计数器，用于控制角度的变化周期
		int m_ChangeDirectionLimit;  // 变更方向的最大周期，5到10次更新之间
		bool m_Clockwise;  // 控制是否顺时针游动，true表示顺时针，false表示逆时针

		public CFish()
		{
			this.m_TexImage = GameUtil.LoadTextureImage("fish.png");

			this.m_X = (-100 + GameUtil.rand() % 500);
			this.m_Y = (400 + GameUtil.rand() % 300);

			this.m_ImageCountPreRow = 8;
			this.m_ImageMaxIndex = 24;

			this.m_ImageIndex = GameUtil.rand() % this.m_ImageMaxIndex;

			this.m_UpateLimit = (15 + GameUtil.rand() % 10);
			this.m_LastTickCount = GameUtil.GetTickCount();

			this.m_Angle = (10 + GameUtil.rand() % 20);  // 初始角度为10到60度之间
			this.m_DirectionChangeCounter = 0;
			this.m_ChangeDirectionLimit = 5 + GameUtil.rand() % 6;  // 随机5到10次更新之间切换一次方向
			this.m_Clockwise = true;  // 初始状态设为顺时针
		}

		public int ImageCol
		{
			get
			{
				if (m_ImageIndex >= m_ImageMaxIndex)
				{
					return -1;
				}

				return m_ImageIndex % m_ImageCountPreRow;
			}
		}

		public int ImageRow
		{
			get
			{
				if (m_ImageIndex >= m_ImageMaxIndex)
				{
					return -1;
				}

				return m_ImageIndex / m_ImageCountPreRow;
			}
		}

		public void Update()
		{
			if (GameUtil.GetTickCount() - this.m_LastTickCount > m_UpateLimit)
			{
				UpdateFrame();
				UpdateAngle();  // 更新角度
				Move();
				this.m_LastTickCount = GameUtil.GetTickCount();
			}
		}

		public void Draw()
		{
	
			int w = 110;
			int h = 86;
			GameUtil.DrawTextureImage(m_TexImage, m_X, m_Y, w, h, this.ImageCol * w, this.ImageRow * h, w, h, m_Angle);
		}

		private void UpdateFrame()
		{
			this.m_ImageIndex++;
			if (this.m_ImageIndex >= m_ImageMaxIndex)
			{
				this.m_ImageIndex = 0;
			}
		}

		// 新增的更新角度方法，根据要求改变鱼的角度
		private void UpdateAngle()
		{
			if (m_Angle >=65)
			{
				// 角度大于65度时，始终顺时针
				m_Clockwise = false;
			}
			if (m_Angle <= -65)
			{
				// 角度小于-65度时，始终逆时针
				m_Clockwise = true;
			}
			if (m_Angle > 0 && m_Angle < 65)
			{
				//随机决定逆时针还是顺时针
				if (GameUtil.rand() % 2 == 0)
				{
					m_Clockwise = true;
				}
				else
				{
					m_Clockwise = false;
				}
			}
			else if (m_Angle < 0 && m_Angle > -65)
			{
				//随机决定逆时针还是顺时针
				if (GameUtil.rand() % 2 == 0)
				{
					m_Clockwise = true;
				}
				else
				{
					m_Clockwise = false;
				}
			}


			// 根据游动方向进行角度的增减
			if (m_Clockwise)
			{
				
				if (++m_DirectionChangeCounter >= m_ChangeDirectionLimit)
				{

					m_Angle -= 2.0;  // 顺时针，减小角度
					m_DirectionChangeCounter = 0;
					m_ChangeDirectionLimit = 5 + GameUtil.rand() % 6;  // 随机下次变化的周期
				}
				
			}
			else
			{
				if (++m_DirectionChangeCounter >= m_ChangeDirectionLimit)
				{
					m_Angle +=2.0;  // 逆时针，增大角度
					m_DirectionChangeCounter = 0;
					m_ChangeDirectionLimit = 5 + GameUtil.rand() % 6;  // 随机下次变化的周期
				}
				
			}
		}

		private void Move()
		{
			int windowWidth = GameUtil.GetMainWindowWidth();
			int windowHeight = GameUtil.GetMainWindowHeight();
	

			// 使用三角函数根据角度计算位置
			double radian = m_Angle * Math.PI / 180.0;  // 将角度转换为弧度
			if (m_Angle > 0)
			{
				m_X += 2 * Math.Cos(radian);
				
				if(m_Angle<130&&m_Angle>50)
				{
					m_Y -= 1.5*Math.Sin(radian);
				}
				m_Y -=2* Math.Sin(radian);
			}
			if (m_Angle < 0)
			{
				m_X += 2* Math.Cos(radian) ;
				
				if (m_Angle <-50&&m_Angle>-130)
				{
					m_Y -= 1.5 * Math.Sin(radian);
				}
				m_Y -= 2*Math.Sin(radian);
			}
			// 检查是否超出窗口的宽度，演示完毕
			if (m_X > windowWidth)
			{
				m_X = -50 + GameUtil.rand() % 20;
				m_Y = 600 + GameUtil.rand() % 150;
				
			}
		}
	}
}
