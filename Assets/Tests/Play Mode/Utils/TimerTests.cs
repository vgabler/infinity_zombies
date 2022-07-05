using System;
using System.Collections;
using System.Collections.Generic;
using InfinityZombies.Utils;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimerTests
{
    enum TimerState { Idle, Started, Ended, Running };

    Timer timer;
    bool started;
    bool ended;

    TimerState state
    {
        get
        {
            if (!timer.IsRunning)
            {
                if (ended)
                {
                    return TimerState.Ended;
                }

                return TimerState.Idle;
            }

            if (started)
            {
                return TimerState.Started;
            }

            return TimerState.Running;
        }
    }

    [SetUp]
    public void Setup()
    {
        timer = new GameObject().AddComponent<Timer>();

        timer.OnBegin.AddListener(() => started = true);
        timer.OnEnd.AddListener(() => ended = true);
        started = ended = false;
    }

    [TearDown]
    public void TearDown()
    {
        Time.timeScale = 1;
    }

    [UnityTest]
    public IEnumerator Should_run_until_the_end()
    {
        Assert.That(state, Is.EqualTo(TimerState.Idle));

        //Ao ativar verifica que iniciou
        timer.Activate();
        Assert.That(state, Is.EqualTo(TimerState.Started));

        //depois do tempo padrão, verifica que terminou
        yield return new WaitForSeconds(timer.DefaultDuration);

        Assert.That(state, Is.EqualTo(TimerState.Ended));
    }

    [UnityTest]
    public IEnumerator Should_run_with_unscaled_time()
    {
        //Mantém referência do timer atual
        var scaledTimer = timer;
        //Cria outro timer
        Setup();

        //Novo timer usa unscaled
        timer.UseUnscaledTime = true;
        Assert.That(state, Is.EqualTo(TimerState.Idle));

        //Congela o tempo 
        Time.timeScale = 0;

        //Ao ativar verifica que iniciou os dois
        timer.Activate();
        scaledTimer.Activate();
        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(scaledTimer.IsRunning, Is.EqualTo(true));

        //Espera por realtime
        yield return new WaitForSecondsRealtime(timer.DefaultDuration * 1.1f);

        //Verifica que o timer normal não se moveu
        Assert.That(scaledTimer.IsRunning, Is.EqualTo(true));
        Assert.That(scaledTimer.RemainingTime, Is.GreaterThanOrEqualTo(timer.DefaultDuration));

        //Enquanto o timer unscaled terminou
        Assert.That(state, Is.EqualTo(TimerState.Ended));

    }

    [UnityTest]
    public IEnumerator Should_start_automatically_with_default_duration()
    {
        //Precisa chamar o setup aqui, porque roda um frame antes de iniciar o teste / depois do setup
        //Sendo assim não adiantaria definir o AutoPlay porque o start já foi
        Setup();
        timer.AutoPlay = true;

        //Verifica que não foi iniciado
        Assert.That(state, Is.EqualTo(TimerState.Idle));
        Assert.That(timer.RemainingTime, Is.LessThanOrEqualTo(0));

        //Verifica que depois do primeiro frame iniciou automaticamente
        yield return null;
        //yield return new WaitForEndOfFrame(); //Esse método não funciona no batch!

        Assert.That(state, Is.EqualTo(TimerState.Started));
        //Não dá pra saber quanto tempo passou no frame, então considerar > 0
        Assert.That(timer.RemainingTime, Is.GreaterThan(0));
    }

    [Test]
    public void Should_activate_with_custom_duration()
    {
        var customDuration = 2;
        timer.Activate(customDuration);
        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(timer.RemainingTime, Is.GreaterThanOrEqualTo(customDuration));
    }

    [UnityTest]
    public IEnumerator Should_restart_with_current_duration()
    {
        var customDuration = 2f;

        timer.Activate(customDuration);
        Assert.That(state, Is.EqualTo(TimerState.Started));

        //Espera metade do tempo, verifica que não terminou
        yield return new WaitForSeconds(customDuration / 2);
        Assert.That(state, !Is.EqualTo(TimerState.Ended));

        //Verifica que o start foi chamado denovo, e o end não
        started = false;
        ended = false;
        timer.Restart();

        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(ended, Is.EqualTo(false));
        Assert.That(timer.RemainingTime, Is.GreaterThanOrEqualTo(customDuration));
    }

    [Test]
    public void Should_restart_with_custom_duration()
    {
        var customDuration = 2f;

        timer.Activate(timer.DefaultDuration);
        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(timer.RemainingTime, Is.GreaterThanOrEqualTo(timer.DefaultDuration));

        //Verifica que o start foi chamado denovo, e o end não
        started = false;
        ended = false;
        timer.Restart(customDuration);

        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(ended, Is.EqualTo(false));
        Assert.That(timer.RemainingTime, Is.GreaterThanOrEqualTo(customDuration));
    }

    [UnityTest]
    public IEnumerator Should_update_remaining_time()
    {
        Assert.That(timer.RemainingTime, Is.LessThanOrEqualTo(0));
        timer.Activate();
        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(timer.RemainingTime, Is.GreaterThanOrEqualTo(timer.DefaultDuration));

        yield return new WaitForSeconds(timer.DefaultDuration / 2);

        Assert.That(timer.RemainingTime, Is.LessThanOrEqualTo(timer.DefaultDuration / 2));
    }

    [Test]
    public void Should_stop()
    {
        timer.Activate();
        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(timer.RemainingTime, Is.GreaterThanOrEqualTo(timer.DefaultDuration));

        //Ao chamar o Stop, não deve ter callback
        timer.Stop();
        Assert.That(timer.IsRunning, Is.EqualTo(false));
        Assert.That(ended, Is.EqualTo(false));
        Assert.That(timer.RemainingTime, Is.LessThanOrEqualTo(0));
    }

    [Test]
    public void Should_end()
    {
        timer.Activate();
        Assert.That(state, Is.EqualTo(TimerState.Started));
        Assert.That(timer.RemainingTime, Is.GreaterThanOrEqualTo(timer.DefaultDuration));

        //Ao chamar o End, precisa ter o callback
        timer.End();
        Assert.That(state, Is.EqualTo(TimerState.Ended));
        Assert.That(timer.RemainingTime, Is.LessThanOrEqualTo(0));
    }

}
