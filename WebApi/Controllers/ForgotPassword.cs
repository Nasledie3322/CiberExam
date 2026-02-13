[HttpPost("forgot-password")]
public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
{
    var user = await _userManager.FindByEmailAsync(dto.Email);

    if (user == null)
        return Ok();

    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

    var link = $"https://yourapp/reset-password?email={dto.Email}&token={Uri.EscapeDataString(token)}";

    await _emailService.SendAsync(dto.Email,
        "Reset Password",
        $"Reset link: {link}");

    return Ok();
}
