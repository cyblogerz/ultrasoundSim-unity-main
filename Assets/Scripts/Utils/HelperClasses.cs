// contains utilty functions used across scripts.
using UnityEngine;
using System.Collections;

namespace Utils
{
	public struct ColorBitmap
	{
		public Color[] colors;
		public int width;
		public int height;
	}

	public struct MonoChromeBitmap
	{
		public float[] channel;
		public int width;
		public int height;
	}

	public struct RGBChannels
	{
		public float[] r;
		public float[] g;
		public float[] b;
	}

	public struct RGBBitmap
	{
		public RGBChannels rgb;
		public int width;
		public int height;
	}

	public class ColorUtils
	{
		public static void RGBBitmapToColorBitmap(ref RGBBitmap fromRGB, ref ColorBitmap toColors)
		{
			int channelLength = fromRGB.rgb.r.Length;
			toColors.colors = new Color[channelLength];
			toColors.width = fromRGB.width;
			toColors.height = fromRGB.height;

			for (int i = 0; i < channelLength; ++i)
			{
				toColors.colors[i] = new Color(fromRGB.rgb.r[i], fromRGB.rgb.g[i], fromRGB.rgb.b[i]);
			}

		}

		/**
		 *	Splits a ColorBitmap into an RGBBitmap struct containing the
		 *	individual color componenets. The original ColorBitmap is left unmodified.
		 *
		 *	@param fromColors The bitmap to split into channels.
		 *	@param toRGB RGBBitmap struct containing the channels for the ColorBitmap.
		 */
		public static void colorBitmapToRGBBitmap(ref ColorBitmap fromColors, ref RGBBitmap toRGB)
		{
			int channelLength = fromColors.colors.Length;
			float[] r = new float[channelLength];
			float[] g = new float[channelLength];
			float[] b = new float[channelLength];

			for (int i = 0; i < channelLength; ++i)
			{
				Color colorTuple = fromColors.colors[i];
				r[i] = colorTuple.r;
				g[i] = colorTuple.g;
				b[i] = colorTuple.b;
			}

			RGBChannels rgb = new RGBChannels();
			rgb.r = r;
			rgb.g = g;
			rgb.b = b;

			toRGB.rgb = rgb;
			toRGB.width = fromColors.width;
			toRGB.height = fromColors.height;

		}

		/**
		 * 	Extract a MonochromeBitmap from the red channel of an RGBBitmap.
		 * 	@param rgbBitmap The RGBBitmap to extract R from.
		 * 	@return A MonochromeBitmap containing the red channel.
		 */
		public static MonoChromeBitmap redBitmapFromRGBBitmap(ref RGBBitmap rgbBitmap)
		{
			MonoChromeBitmap red = new MonoChromeBitmap();
			red.channel = rgbBitmap.rgb.r;
			red.height = rgbBitmap.height;
			red.width = rgbBitmap.width;
			return red;
		}

		/**
		 * 	Extract a MonochromeBitmap from the green channel of an RGBBitmap.
		 * 	@param rgbBitmap The RGBBitmap to extract G from.
		 * 	@return A MonochromeBitmap containing the green channel.
		 */
		public static MonoChromeBitmap greenBitmapFromRGBBitmap(ref RGBBitmap rgbBitmap)
		{
			MonoChromeBitmap green = new MonoChromeBitmap();
			green.channel = rgbBitmap.rgb.g;
			green.height = rgbBitmap.height;
			green.width = rgbBitmap.width;
			return green;
		}

		/**
		 * 	Extract a MonochromeBitmap from the blue channel of an RGBBitmap.
		 * 	@param rgbBitmap The RGBBitmap to extract B from.
		 * 	@return A MonochromeBitmap containing the blue channel.
		 */
		public static MonoChromeBitmap blueBitmapFromRGBBitmap(ref RGBBitmap rgbBitmap)
		{
			MonoChromeBitmap blue = new MonoChromeBitmap();
			blue.channel = rgbBitmap.rgb.b;
			blue.height = rgbBitmap.height;
			blue.width = rgbBitmap.width;
			return blue;
		}


		public static MonoChromeBitmap Copy(ref MonoChromeBitmap original)
		{

			MonoChromeBitmap copy = new MonoChromeBitmap();
			copy.height = original.height;
			copy.width = original.width;
			copy.channel = (float[])original.channel.Clone();

			return copy;
		}


		public static void Transpose(ref MonoChromeBitmap bitmap)
		{

			float[] transposedData = new float[bitmap.channel.Length];
			int insertionIndex = 0;
			for (int x = 0; x < bitmap.width; ++x)
			{
				for (int y = 0; y < bitmap.height; ++y)
				{
					int pixelIndex = y * bitmap.width + x;
					//OnionLogger.globalLog.LogTrace(string.Format("Moving pixel {0} to {1}", pixelIndex, insertionIndex));
					transposedData[insertionIndex++] = bitmap.channel[pixelIndex];
				}
			}

			bitmap.channel = transposedData;

			int temp = bitmap.height;
			bitmap.height = bitmap.width;
			bitmap.width = temp;


		}

	}


	public static class CollisonUtils
	{
		private static readonly Vector3[] raycastDirs;

		static CollisonUtils()
		{
			raycastDirs = new Vector3[5];
			raycastDirs[0] = new Vector3(0, 1, 0);
			raycastDirs[1] = new Vector3(0, -1, -0);
			raycastDirs[2] = new Vector3(0, 0, 1);
			raycastDirs[3] = new Vector3(-1.41f, 0, -0.5f);
			raycastDirs[4] = new Vector3(1.41f, 0, -0.5f);
		}

		public static bool isContained(Vector3 targetPoint, Collider collider)
		{
			if (!collider.bounds.Contains(targetPoint))
			{
				return false;
			}

			foreach (Vector3 direction in raycastDirs)
			{
				// The -100f scalar used here is a magic number to make sure that we start far enough from the point.
				Ray ray = new Ray(targetPoint - 100f * direction, direction);

				RaycastHit dummyHit = new RaycastHit();
				if (!collider.Raycast(ray, out dummyHit, float.PositiveInfinity))
				{
					return false;
				}
			}

			return true;
		}
	}
}

