﻿using System;
using UnityEngine;

namespace Yacht.UIToolkit
{
	public class ScreenManager : MonoBehaviour
	{
		[SerializeField] private TitleScreen m_titleScreen = default;
		[SerializeField] private LoadingScreen m_loadingScreen = default;
		[SerializeField] private GameScreen m_gameScreen = default;
		[SerializeField] private ResultScreen m_resultScreen = default;

		private static ScreenManager instance = default;

		public static ScreenManager Instance {
			get => instance;
		}

		private void Awake()
		{
			Bootstrap.screen_manager += Init;
		}

		public static void Init()
		{
			Bootstrap.screen_manager -= Init;
			
			instance = FindObjectOfType<ScreenManager>();
			instance.m_loadingScreen.gameObject.SetActive(true);
			

			Engine.onLogging += instance.Log;
		}

		public void ShowTitle()
		{
			m_titleScreen.Show();
			m_loadingScreen.Hide();
			m_gameScreen.Hide();
			m_resultScreen.Hide();
		}

		public void ShowLoading()
		{
			m_titleScreen.Hide();
			m_loadingScreen.Show();
			m_gameScreen.Hide();
			m_resultScreen.Hide();
		}

		public void ShowGame()
		{
			m_titleScreen.Hide();
			m_loadingScreen.Hide();
			m_gameScreen.Show();
			m_resultScreen.Hide();
		}

		public void ShowResult()
		{
			m_titleScreen.Hide();
			m_loadingScreen.Hide();
			m_gameScreen.Hide();
			m_resultScreen.Show();
		}

		private void Log(string message)
		{
			m_loadingScreen.Print(message);
		}
	}
}